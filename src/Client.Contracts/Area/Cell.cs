using System.Collections.Generic;
using Client.Contracts.Entities;

namespace Client.Contracts.Area
{
	public class Cell
	{
		public int X { get; set; }
		public int Y { get; set; }
		public bool IsWalkable { get; set; }
		public List<Entity> Creatures { get; set; }
		public List<Entity> Items { get; set; }

	}
}