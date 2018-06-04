using System;

namespace Darwin.Api.Actions
{
	public class ActionResponse
	{
		public bool ActionQueued { get; set; }
		public string CurrentActionQueued { get; set; }
		public DateTime NextResolveTime { get; set; }
		public DateTime CurrentTime { get; internal set; }
	}
}