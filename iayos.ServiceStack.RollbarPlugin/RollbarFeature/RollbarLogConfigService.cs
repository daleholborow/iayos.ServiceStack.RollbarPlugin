using System;
using System.Linq;
using ServiceStack;
using ServiceStack.Web;

namespace iayos.ServiceStack.RollbarPlugin.RollbarFeature
{
	[Restrict(VisibilityTo = RequestAttributes.None)]
	public class RollbarLogConfigService : Service
	{
		public RollbarLogConfigRequest Any(RollbarLogConfigRequest req)
		{
			// resolve request logger
			if (!(TryResolve<IRequestLogger>() is RollbarRequestLogger logger)) throw new Exception("Could not resolve RollbarRequestLogger");

			// restrict permissions to roles if configured
			if (logger.RequiredRoles.Any())
			{
				var session = GetSession();
				if (session != null && !logger.RequiredRoles.Any(t => session.HasRole(t, base.AuthRepository)))
				{
					return null;
				}
			}

			if (req.Enabled.HasValue) logger.IsLoggingEnabled = req.Enabled.Value;
			if (req.EnableErrorTracking.HasValue) logger.EnableErrorTracking = req.EnableErrorTracking.Value;
			if (req.EnableRequestBodyTracking.HasValue) logger.EnableRequestBodyTracking = req.EnableRequestBodyTracking.Value;
			if (req.EnableResponseTracking.HasValue)
			{
				if (req.EnableRequestBodyTracking != null && !req.EnableRequestBodyTracking.Value)
				{
					logger.EnableResponseTracking = req.EnableResponseTracking.Value;
				}
				else
				{
					if (HostContext.GetPlugin<RollbarLoggerPlugin>().EnableResponseTracking)
					{
						logger.EnableResponseTracking = true;
					}
					else
					{
						throw new Exception(
							"EnableResponseTracking cannot be enabled if not initially requested at AppHost startup. This feature requires PreRequestFilters");
					}
				}
			}

			if (req.EnableSessionTracking.HasValue) logger.EnableSessionTracking = req.EnableSessionTracking.Value;
			return logger.ConvertTo<RollbarLogConfigRequest>();
		}
	}
}