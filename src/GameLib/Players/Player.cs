using GameLib.Actions.Movement;
using GameLib.Area;

namespace GameLib.Players
{
	public class Player : Entity
	{
		public GameState GameState { get; set; }

		public Player()
		{
			Position = new Position();
			GameState = GameState.lobby;
			Type = nameof(Player);
		}
	}
}