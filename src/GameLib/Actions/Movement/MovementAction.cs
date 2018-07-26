namespace GameLib.Actions.Movement
{
  public enum MovementDirection { West, East, North, South }

  public class MovementAction : Action
  {
    public static string CanonicalName => "Action.Movement";
    public MovementDirection MovementDirection { get; set; }

    public MovementAction(string ownerId, MovementDirection direction)
      : base(ownerId, name: CanonicalName, cooldownDurationMilisecs: 100)
    {
      MovementDirection = direction;
    }

    public override string ToString()
    {
      return base.ToString() + $", {nameof(MovementDirection)}: '{MovementDirection}'";
    }
  }
}