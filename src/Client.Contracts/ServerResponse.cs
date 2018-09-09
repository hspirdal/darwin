using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Client.Contracts
{
	public class ServerResponse
	{
		public ServerResponse()
		{
			Type = ResponseType.InvalidType;
			Message = string.Empty;
			Payload = string.Empty;
		}
		public ServerResponse(ResponseType type, string message)
		{
			Type = type;
			Message = message;
			Payload = string.Empty;
		}

		public ServerResponse(ResponseType type, string message, string payload)
		{
			Type = type;
			Message = message;
			Payload = payload;
		}

		[JsonConverter(typeof(StringEnumConverter))]
		public ResponseType Type { get; set; }
		public string Message { get; set; }
		public string Payload { get; set; }
	}
}