namespace TcpGameClient
{
	public enum MovementDirection { West, East, North, South }

	public class MovementAction
	{
		public int OwnerId { get; set; }
		public string Name { get; set; }
		public MovementDirection MovementDirection { get; set; }
	}
}