using Client.Contracts.Properties;

namespace Client.Contracts.Entities
{
	public class Player
	{
		public string Name { get; set; }
		public Inventory Inventory { get; set; }
	}
}