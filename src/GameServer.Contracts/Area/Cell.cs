namespace TcpGameServer.Contracts.Area
{
  public class Cell
  {
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsWalkable { get; set; }
    public Entity Visitor { get; set; }

  }

  public class Entity
  {
    public string Name { get; set; }
    public string Type { get; set; }
  }
}