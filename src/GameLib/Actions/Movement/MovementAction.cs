namespace GameLib.Actions.Movement
{
	public enum MovementDirection { West, East, North, South }

	public class MovementAction : Action
	{
		public MovementDirection MovementDirection { get; set; }

		public override bool IsValid()
		{
			return base.IsValid();
		}

		public override string ToString()
		{
			return base.ToString() + $", {nameof(MovementDirection)}: '{MovementDirection}'";
		}
	}
}