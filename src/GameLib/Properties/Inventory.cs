using System.Collections.Generic;
using GameLib.Entities;

namespace GameLib.Properties
{
	public class Inventory
	{
		public List<Item> Items { get; set; }

		public Inventory()
		{
			Items = new List<Item>();
		}
	}
}