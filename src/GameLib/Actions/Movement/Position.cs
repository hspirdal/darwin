namespace GameLib.Actions.Movement
{
  public class Position
  {
    public int X { get; set; }
    public int Y { get; set; }

    public void SetPosition(int x, int y)
    {
      X = x;
      Y = y;
    }

    public void Move(int x, int y)
    {
      X += x;
      Y += y;
    }
  }
}