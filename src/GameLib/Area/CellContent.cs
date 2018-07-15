using System.Collections.Generic;

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