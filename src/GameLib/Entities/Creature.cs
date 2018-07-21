using GameLib.Properties;
using GameLib.Properties.Stats;

namespace GameLib.Entities
{
	public class Creature : Entity
	{
		public string Race { get; set; }
		public string Class { get; set; }
		public Statistics Statistics { get; set; }

		public Creature()
			: base(id: string.Empty, name: string.Empty, type: nameof(Creature))
		{
			Race = string.Empty;
			Class = string.Empty;
			Statistics = new Statistics();
		}

		public Creature(string id, string name, string race, string entityClass, string type, Statistics statistics)
			: base(id, name, type)
		{
			Race = race;
			Class = entityClass;
			Statistics = statistics;
		}
	}
}