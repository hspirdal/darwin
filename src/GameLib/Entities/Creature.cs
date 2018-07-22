using System;
using GameLib.Properties;
using GameLib.Properties.Stats;
using GameLib.Utility;

namespace GameLib.Entities
{
	public class Creature : Entity, IDeepCopy<Creature>
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

		public Creature(string id, string name, string race, string entityClass, Statistics statistics)
	: base(id, name, type: nameof(Creature))
		{
			Race = race;
			Class = entityClass;
			Statistics = statistics;
		}

		public Creature DeepCopy()
		{
			return new Creature(string.Copy(Id), string.Copy(Name), string.Copy(Race), string.Copy(Class), string.Copy(Type), Statistics.DeepCopy());
		}
	}
}