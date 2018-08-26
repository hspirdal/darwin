using System;
using GameLib.Properties;

namespace GameLib.Users
{
	public class User : IIdentifiable
	{
		public string Id { get; set; }
		public Guid SessionId { get; set; }
		public GameState GameState { get; set; }
	}
}