using System.Collections.Generic;

namespace iayos.ServiceStack.RollbarPlugin.RollbarFeature
{
	/// <summary>
	/// Encapsulate all the appsettings.json configuration for the Rollbar RequestLogger feature
	/// </summary>
	public class RollbarSettings
	{
		public string ApiKey { get; set; }

		public bool Enabled { get; set; }

		public bool EnableErrorTracking { get; set; }

		public bool EnableRequestBodyTracking { get; set; }

		public bool EnableSessionTracking { get; set; }

		public bool EnableResponseTracking { get; set; }

		public List<string> RequiredRoles { get; set; } = new List<string>();


		/// <summary>
		/// Manually set the environment to track the events origin/host
		/// </summary>
		public string Environment { get; set; } = "Production";
	}
}