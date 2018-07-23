using GameLib.Properties;
using GameLib.Properties.Stats;

namespace GameLib.Entities
{
	public class Player : Creature
	{
		public GameState GameState { get; set; } //  TODO: Move out.
		public AttributeScore Level { get; set; }
		public Inventory Inventory { get; set; }

		public Player()
			: base(id: string.Empty, name: string.Empty, race: string.Empty, entityClass: string.Empty, type: nameof(Player), new Position(), new Statistics())
		{
			Level = new AttributeScore { Base = 0 };
			GameState = GameState.lobby;
			Inventory = new Inventory();
		}

		public Player(string id, string name, string race, string entityClass, int level, Statistics statistics)
		: base(id, name, race, entityClass, type: nameof(Player), new Position(), statistics)
		{
			Level = new AttributeScore { Base = level };
			GameState = GameState.lobby;
			Inventory = new Inventory();
		}
	}
}