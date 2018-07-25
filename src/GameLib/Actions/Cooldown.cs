using System;

namespace GameLib.Actions
{
	public class Cooldown
	{
		public string UserId { get; private set; }
		public DateTime ValidTo { get; private set; }

		public Cooldown(string userId, DateTime validTo)
		{
			UserId = userId;
			ValidTo = validTo;
		}
	}
}