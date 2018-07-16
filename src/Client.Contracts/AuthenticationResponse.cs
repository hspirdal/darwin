using System;

namespace Client.Contracts
{
	public class AuthenticationResponse
	{
		public bool Success { get; set; }
		public Guid SessionId { get; set; }
	}
}