namespace Darwin.Api.Actions.Movement
{
	public class MovementAction : IAction
	{
		private readonly IMovementResolver _resolver;

		public MovementAction(IMovementResolver resolver, int ownerId, MovementDirection movementDirection)
		{
			_resolver = resolver;
			OwnerId = ownerId;
			MovementDirection = movementDirection;
		}

		public int OwnerId { get; private set; }
		public string Name => nameof(MovementAction);
		public MovementDirection MovementDirection { get; private set; }

		public void Resolve()
		{
			_resolver.Resolve(OwnerId, MovementDirection);
		}
	}
}