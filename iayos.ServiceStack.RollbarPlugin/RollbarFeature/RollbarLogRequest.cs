using System.Runtime.Serialization;
using iayos.ServiceStack.RollbarPlugin.AdaptedFromRollbar;
using ServiceStack;

namespace iayos.ServiceStack.RollbarPlugin.RollbarFeature
{

	//[Route("/api/{version}/item/")]
	public class RollbarLogRequest : IReturnVoid 
	{

		[DataMember(Name = "access_token")]
		public string AccessToken { get; set; }

		public Data Data { get; set; }

		public int Version { get; set; } = 1;
	}
}