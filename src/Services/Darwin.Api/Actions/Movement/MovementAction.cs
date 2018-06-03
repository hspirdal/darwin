namespace Darwin.Api.Actions.Movement
{
	public class MovementAction : IAction
	{
		public MovementAction(int ownerId, MovementDirection movementDirection)
		{
			OwnerId = ownerId;
			MovementDirection = movementDirection;
		}

		public int OwnerId { get; private set; }
		public string Name => nameof(MovementAction);
		public MovementDirection MovementDirection { get; private set; }
	}
}