using Client.Contracts.Properties;
using Client.Contracts.Properties.Stats;

namespace Client.Contracts.Entities
{
	public class Player
	{
		public string Name { get; set; }
		public CharacterStatistics Statistics { get; set; }
		public Inventory Inventory { get; set; }
	}
}