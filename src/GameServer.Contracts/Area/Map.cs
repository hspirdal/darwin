using System.Collections.Generic;

namespace TcpGameServer.Contracts.Area
{
	public class Map
	{
		public List<Cell> VisibleCells { get; set; }

		public int Width { get; set; }
		public int Height { get; set; }
	}
}