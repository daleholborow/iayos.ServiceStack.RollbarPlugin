using ServiceStack;

namespace iayos.ServiceStack.RollbarPlugin.RollbarFeature
{

	[Route("/RollbarLogConfig")]
	public class RollbarLogConfigRequest : IReturn<RollbarLogConfigRequest>
	{
		public bool? Enabled { get; set; }

		public bool? EnableSessionTracking { get; set; }

		public bool? EnableRequestBodyTracking { get; set; }

		public bool? EnableResponseTracking { get; set; }

		public bool? EnableErrorTracking { get; set; }
	}
}
