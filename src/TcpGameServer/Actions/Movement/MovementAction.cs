namespace TcpGameServer.Actions.Movement
{
	public enum MovementDirection { West, East, North, South }

	public class MovementAction : IAction
	{
		public int OwnerId { get; set; }
		public string Name { get; set; }
		public MovementDirection MovementDirection { get; set; }

		public void Resolve()
		{

		}
	}
}