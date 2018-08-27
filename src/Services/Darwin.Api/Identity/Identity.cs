using System;
using GameLib.Properties;

namespace Darwin.Api.Identities
{
	public class Identity
	{
		public string Id { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public Guid SessionId { get; set; }
	}
}