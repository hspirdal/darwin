using Newtonsoft.Json;

namespace Client.Contracts
{
	public class ServerResponse
	{
		public ServerResponse()
		{
			Type = string.Empty;
			Message = string.Empty;
			Payload = string.Empty;
		}
		public ServerResponse(string message)
		{
			Type = string.Empty;
			Message = message;
			Payload = string.Empty;
		}

		public string Type { get; set; }
		public string Message { get; set; }
		public string Payload { get; set; }
	}
}