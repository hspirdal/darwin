using TcpGameServer.Contracts.Entities;
using TcpGameServer.Contracts.Area;

namespace TcpGameServer.Contracts.Properties
{
	public class GameStatus
	{
		public int X { get; set; }
		public int Y { get; set; }
		public Player Player { get; set; }
		public Map Map { get; set; }
	}
}