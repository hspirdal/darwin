using Client.Contracts.Properties.Stats;

namespace Client.Contracts.Entities
{
	public class Creature : Entity
	{
		public string Race { get; set; }
		public string Class { get; set; }
		public Statistics Statistics { get; set; }
	}
}