using Client.Contracts.Entities;
using Client.Contracts.Area;

namespace Client.Contracts.Properties
{
	public class GameStatus
	{
		public int X { get; set; }
		public int Y { get; set; }
		public Player Player { get; set; }
		public Map Map { get; set; }
	}
}