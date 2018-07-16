using GameLib.Properties;
using GameLib.Properties.Stats;

namespace GameLib.Entities
{
	public class Player : Entity
	{
		public GameState GameState { get; set; }
		public CharacterStatistics Statistics { get; set; }
		public Inventory Inventory { get; set; }
		public Position Position { get; set; }

		public Player()
		{
			GameState = GameState.lobby;
			Statistics = new CharacterStatistics();
			Position = new Position();
			Inventory = new Inventory();
			Type = nameof(Player);
		}
	}
}