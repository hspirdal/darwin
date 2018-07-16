using GameLib.Properties;

namespace GameLib.Entities
{
	public class Player : Entity
	{
		public GameState GameState { get; set; }
		public Inventory Inventory { get; set; }
		public Position Position { get; set; }

		public Player()
		{
			Position = new Position();
			Inventory = new Inventory();
			GameState = GameState.lobby;
			Type = nameof(Player);
		}
	}
}