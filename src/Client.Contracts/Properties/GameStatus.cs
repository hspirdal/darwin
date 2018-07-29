using Client.Contracts.Entities;
using Client.Contracts.Area;
using System.Collections.Generic;
using System;

namespace Client.Contracts.Properties
{
	public class GameStatus
	{
		public int X { get; set; }
		public int Y { get; set; }
		public bool IsInCombat { get; set; }
		public Player Player { get; set; }
		public Map Map { get; set; }
		public DateTime NextActionAvailableUtc { get; set; }
	}
}