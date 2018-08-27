using System;

namespace Darwin.Api.Identities
{
	public class LoginResponse
	{
		public bool Success { get; set; }
		public string UserId { get; set; }
		public Guid SessionId { get; set; }
	}
}