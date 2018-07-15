using System.Collections.Generic;

namespace TcpGameServer.Contracts.Area
{
	public class Cell
	{
		public int X { get; set; }
		public int Y { get; set; }
		public bool IsWalkable { get; set; }
		public List<Entity> Content { get; set; }

	}
}