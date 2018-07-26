namespace GameLib.Actions.Combat
{
  public class AttackAction : Action
  {
    public static string CanonicalName => "Action.Attack";
    public string TargetId { get; set; }

    public AttackAction(string ownerId, string targetId)
      : base(ownerId, name: CanonicalName, cooldownDurationMilisecs: 1000)
    {
      TargetId = targetId;
    }

    public override string ToString()
    {
      return base.ToString() + $", {nameof(TargetId)}: '{TargetId}'";
    }
  }
}