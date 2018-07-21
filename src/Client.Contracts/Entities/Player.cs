using Client.Contracts.Properties;
using Client.Contracts.Properties.Stats;

namespace Client.Contracts.Entities
{
	public class Player : Creature
	{
		public int Level { get; set; }
		public Inventory Inventory { get; set; }
	}
}