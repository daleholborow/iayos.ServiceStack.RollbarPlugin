using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace iayos.ServiceStack.RollbarPlugin.AdaptedFromRollbar
{
	/// <summary>
	///     Models Rollbar Body DTO.
	/// </summary>
	/// <seealso cref="DtoBase" />
	public class Body
		: DtoBase
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="Body" /> class.
		/// </summary>
		/// <param name="exceptions">The exceptions.</param>
		public Body(IEnumerable<Exception> exceptions)
		{
			Assumption.AssertNotNullOrEmpty(exceptions, nameof(exceptions));

			var allExceptions = exceptions as Exception[] ?? exceptions.ToArray();
			TraceChain = allExceptions.Select(e => new Trace(e)).ToArray();

			Validate();
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Body" /> class.
		/// </summary>
		/// <param name="exception">The exception.</param>
		public Body(AggregateException exception)
			: this(exception.InnerExceptions)
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Body" /> class.
		/// </summary>
		/// <param name="exception">The exception.</param>
		public Body(Exception exception)
		{
			Assumption.AssertNotNull(exception, nameof(exception));

			if (exception.InnerException != null)
			{
				var exceptionList = new List<Exception>();
				var outerException = exception;
				while (outerException != null)
				{
					exceptionList.Add(outerException);
					outerException = outerException.InnerException;
				}

				var traceChain = exceptionList.Select(e => new Trace(e)).ToArray();
				TraceChain = traceChain;
			}
			else
			{
				Trace = new Trace(exception);
				TraceChain = null; // Force null so that Rollbar doesn't flip out
			}

			Validate();
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Body" /> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public Body(Message message)
		{
			Assumption.AssertNotNull(message, nameof(message));

			Message = message;
			Validate();
			TraceChain = null; // Force null so that Rollbar doesn't flip out
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Body" /> class.
		/// </summary>
		/// <param name="crashReport">The crash report.</param>
		public Body(string crashReport)
		{
			Assumption.AssertNotNullOrWhiteSpace(crashReport, nameof(crashReport));

			CrashReport = new CrashReport(crashReport);
			TraceChain = null; // Force null so that Rollbar doesn't flip out
			Validate();
		}

		/// <summary>
		///     Validates this instance.
		/// </summary>
		public override void Validate()
		{
			var bodyContentVariationsCount = 0;

			if (Trace != null)
			{
				Trace.Validate();
				bodyContentVariationsCount++;
			}

			if (TraceChain != null && TraceChain.Length > 0) bodyContentVariationsCount++;
			if (Message != null)
			{
				Message.Validate();
				bodyContentVariationsCount++;
			}

			if (CrashReport != null) bodyContentVariationsCount++;

			Assumption.AssertEqual(bodyContentVariationsCount, 1, nameof(bodyContentVariationsCount));
		}

		#region These are mutually exclusive properties - only one of them can be not null

		/// <summary>
		///     Gets the trace.
		/// </summary>
		/// <value>
		///     The trace.
		/// </value>
		//[JsonProperty("trace", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public Trace Trace { get; }

		/// <summary>
		///     Gets the trace chain.
		/// </summary>
		/// <value>
		///     The trace chain.
		/// </value>
		//[JsonProperty("trace_chain", DefaultValueHandling = DefaultValueHandling.Ignore)]
		[DataMember(Name = "trace_chain")]
		public Trace[] TraceChain { get; } //= new List<Trace>();

		/// <summary>
		///     Gets the message.
		/// </summary>
		/// <value>
		///     The message.
		/// </value>
		//[JsonProperty("message", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public Message Message { get; }

		/// <summary>
		///     Gets the crash report.
		/// </summary>
		/// <value>
		///     The crash report.
		/// </value>
		//[JsonProperty("crash_report", DefaultValueHandling = DefaultValueHandling.Ignore)]
		[DataMember(Name = "crash_report")]
		public CrashReport CrashReport { get; }

		#endregion These are mutually exclusive properties - only one of them can be not null
	}
}