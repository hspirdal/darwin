using System;

namespace Client.Contracts
{
	public class ClientRequest
	{
		public string RequestName { get; set; }
		public string Payload { get; set; }
		public Guid SessionId { get; set; }
	}
}
