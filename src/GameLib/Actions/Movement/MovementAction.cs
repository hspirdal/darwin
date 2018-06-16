namespace GameLib.Actions.Movement
{
	public enum MovementDirection { West, East, North, South }

	public class MovementAction : Action
	{
		public MovementDirection MovementDirection { get; set; }
	}
}