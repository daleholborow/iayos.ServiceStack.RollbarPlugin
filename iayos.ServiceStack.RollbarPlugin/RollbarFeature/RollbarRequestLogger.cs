using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using iayos.ServiceStack.RollbarPlugin.AdaptedFromRollbar;
using ServiceStack;
using ServiceStack.Logging;
using ServiceStack.Text;
using ServiceStack.Web;

namespace iayos.ServiceStack.RollbarPlugin.RollbarFeature
{

	public class RollbarRequestLogger : IRequestLogger
	{

		private readonly RollbarLoggerPlugin _plugin;

		public RollbarRequestLogger(RollbarLoggerPlugin plugin)
		{
			_plugin = plugin;

			// The request message MUST be serialized to omit NULL TraceChain and other properties. 
			// We enforce the custom configuration here, so that regardless of what AppHost setting we have, 
			// the logger will use the appropriate serialization strategy
			JsConfig<RollbarLogRequest>.RawSerializeFn = (obj) =>
			{
				using (var config = JsConfig.BeginScope())
				{
					config.EmitCamelCaseNames = true;
					config.IncludeNullValues = false;
					return obj.ToJson();
				}
			};
		}

		
		private void BufferedLogEntries(RollbarLogRequest rollbarLogRequest)
		{
			// TODO add buffering to logging for perf

			try
			{
				"https://api.rollbar.com/api/1/item/".PostJsonToUrlAsync(rollbarLogRequest);
				//_client.PostAsync(rollbarLogRequest);
			}
			catch (Exception)
			{
				// Couldn't connect to Rollbar or the logging failed for some reason. 
				// For now, I'll assume that we fail out silently and ponder the wisdom of this later...
			}
		}
		

		#region Bind settings to the plugin config settings

		public bool IsLoggingEnabled
		{
			get => _plugin.Enabled;
			set => _plugin.Enabled = value;
		}

		public bool EnableSessionTracking
		{
			get => _plugin.EnableSessionTracking;
			set => _plugin.EnableSessionTracking = value;
		}

		public bool EnableRequestBodyTracking
		{
			get => _plugin.EnableRequestBodyTracking;
			set => _plugin.EnableRequestBodyTracking = value;
		}

		public bool EnableResponseTracking
		{
			get => _plugin.EnableResponseTracking;
			set => _plugin.EnableResponseTracking = value;
		}

		public bool EnableErrorTracking
		{
			get => _plugin.EnableErrorTracking;
			set => _plugin.EnableErrorTracking = value;
		}

		public string[] RequiredRoles
		{
			get => _plugin.RequiredRoles?.ToArray();
			set => _plugin.RequiredRoles = value?.ToList();
		}

		public Type[] ExcludeRequestDtoTypes
		{
			get => _plugin.ExcludeRequestDtoTypes?.ToArray();
			set => _plugin.ExcludeRequestDtoTypes = value?.ToList();
		}

		public Type[] HideRequestBodyForRequestDtoTypes
		{
			get => _plugin.HideRequestBodyForRequestDtoTypes?.ToArray();
			set => _plugin.HideRequestBodyForRequestDtoTypes = value?.ToList();
		}

		/// <summary>
		/// Input: request, requestMessage, responseMessage, requestDuration
		/// Output: List of Properties to append to Rollbar Log entry
		/// </summary>
		public RollbarLoggerPlugin.PropertyAppender AppendProperties
		{
			get => _plugin.AppendProperties;
			set => _plugin.AppendProperties = value;
		}

		#endregion Bind settings to the plugin config settings
		

		public void Log(IRequest request, object requestDto, object response, TimeSpan requestDuration)
		{
			try
			{
				// bypasses all flags to run raw log event delegate if configured
				_plugin.RawEventLogger?.Invoke(request, requestDto, response, requestDuration);

				// if logging disabled
				if (!IsLoggingEnabled) return;

				// check any custom filter
				if (_plugin.SkipLogging?.Invoke(request) == true) return;

				// skip logging any dto exclusion types set
				var requestType = requestDto?.GetType();
				if (ExcludeRequestType(requestType)) return;

				var entry = CreateEntry(request, requestDto, response, requestDuration, requestType);
				BufferedLogEntries(entry);
			}
			catch (Exception ex)
			{
				LogManager.GetLogger(typeof(RollbarRequestLogger))
					.Error("RollbarRequestLogger threw unexpected exception", ex);
			}
		}


		public List<RequestLogEntry> GetLatestLogs(int? take)
		{
			// use rollbar browser for reading logs
			throw new NotSupportedException($"use rollbar website for reading logs - its very fully featured");
		}


		protected RollbarLogRequest CreateEntry(
			IRequest request,
			object requestMessage,
			object responseMessage,
			TimeSpan requestDuration,
			Type requestType)
		{

			var rollbarConfig = new RollbarConfig(_plugin.ApiKey)
			{
				Environment = _plugin.Environment
			};

			var messageErrorLevel = ErrorLevel.Info;

			var loggableRequest = new Request();

			Person loggablePerson = null;

			if (request != null)
			{
				if (EnableSessionTracking)
				{
					var session = request.GetSession();
					loggablePerson = new Person
					{
						Id = session.UserAuthId,
						Email = session.Email,
						UserName = session.UserName
					};
				}

				loggableRequest = new Request
				{
					Url = request.AbsoluteUri,
					Method = request.Verb,
					Headers = request.Headers.ToDictionary(),
					// TODO set up Route params if using with MVC. I dont use MVC, so I haven't bothered
					Params = new Dictionary<string, object>(),
					UserIp = request.UserHostAddress
				};
				if (request.Verb == HttpMethods.Get)
				{
					loggableRequest.GetParams = new Dictionary<string, object>();
					foreach (var x in request.GetRequestParams())
					{
						loggableRequest.GetParams.Add(new KeyValuePair<string, object>(x.Key, x.Value));
					}
					loggableRequest.QueryString = request.PathInfo;
				}
				else if (request.Verb == HttpMethods.Post)
				{
					loggableRequest.PostParams = new Dictionary<string, object>();
					foreach (var x in request.GetRequestParams())
					{
						loggableRequest.PostParams.Add(new KeyValuePair<string, object>(x.Key, x.Value));
					}
				}

				var isClosed = request?.Response?.IsClosed ?? false;
				if (!isClosed)
				{
					loggableRequest.UserAuthId = request.GetItemOrCookie(HttpHeaders.XUserAuthId);
					loggableRequest.SessionId = request.GetSessionId();
				}

				if (HideRequestBodyForRequestDtoTypes != null
				    && requestType != null
				    && !HideRequestBodyForRequestDtoTypes.Contains(requestType))
				{
					if (!isClosed)
					{
						loggableRequest.PostBody = request.FormData.ToDictionary().ToJson();
					}

					if (EnableRequestBodyTracking)
					{
						loggableRequest.PostBody = request.GetRawBody();
					}
				}
			}

			var loggableBody = new Body("Developer Error: some strange condition occurred in the Rollbar request logging plugin");
			if (!responseMessage.IsErrorResponse())
			{
				Message loggableMessage;
				if (EnableResponseTracking)
				{
					var messageBody = $"HTTP {request?.Verb} {request?.PathInfo} responded {request?.Response?.StatusCode} in {requestDuration.TotalMilliseconds}ms";
					loggableMessage = new Message(messageBody, responseMessage.ToSafePartialObjectDictionary());
				}
				else
				{
					loggableMessage = new Message("Response Tracking currently disabled");
				}
				loggableBody = new Body(loggableMessage);
			}
			else 
			{
				if (EnableErrorTracking)
				{
					if (responseMessage is IHttpError errorResponse)
					{
						messageErrorLevel = errorResponse.StatusCode >= HttpStatusCode.BadRequest
						                    && errorResponse.StatusCode < HttpStatusCode.InternalServerError
							? ErrorLevel.Warning
							: ErrorLevel.Error;

						var messageTxt = $"HttpError {errorResponse.StatusCode} : {errorResponse.ErrorCode} : {errorResponse.Message}";
						var loggableMessage = new Message(messageTxt, errorResponse.ToSafePartialObjectDictionary());
						loggableBody = new Body(loggableMessage);
					}
					else if (responseMessage is Exception ex)
					{
						loggableBody = new Body(ex);
						messageErrorLevel = ErrorLevel.Critical;
					}
				}
				else
				{
					messageErrorLevel = ErrorLevel.Error;
					var loggableMessage = new Message("Error detected but error tracking currently disabled.");
					loggableBody = new Body(loggableMessage);
				}
			}

			var data = new Data(
				rollbarConfig,
				loggableBody
			)
			{
				Context = request?.GetType().Name,
				Request = loggableRequest,
				Level = messageErrorLevel,
				Person = loggablePerson
			};

			var rollbarLogRequest = new RollbarLogRequest
			{
				AccessToken = _plugin.ApiKey,
				Data = data
			};

			if (AppendProperties != null)
			{
				foreach (var kvPair in AppendProperties?.Invoke(request, requestMessage, responseMessage, requestDuration).Safe())
				{
					rollbarLogRequest.Data.Custom.Add(kvPair.Key, kvPair.Value);
				}
			}
			return rollbarLogRequest;
		}


		protected bool ExcludeRequestType(Type requestType)
		{
			return ExcludeRequestDtoTypes != null
			       && requestType != null
			       && ExcludeRequestDtoTypes.Contains(requestType);
		}


		public bool LimitToServiceRequests { get; set; }


		public Func<IRequest, bool> SkipLogging { get; set; }

	}

}
