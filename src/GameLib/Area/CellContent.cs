using System.Collections.Generic;
using GameLib.Entities;

namespace GameLib.Area
{
	public class CellContent
	{
		public List<Entity> Entities { get; set; }

		public CellContent()
		{
			Entities = new List<Entity>();
		}
	}
}