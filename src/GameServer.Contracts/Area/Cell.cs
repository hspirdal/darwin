using System.Collections.Generic;
using TcpGameServer.Contracts.Entities;

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