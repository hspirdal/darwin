using GameLib.Actions.Movement;

namespace GameLib.Players
{
  public class Player
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public GameState GameState { get; set; }
    public Position Position { get; set; }

    public Player()
    {
      Position = new Position();
      GameState = GameState.lobby;
    }
  }
}