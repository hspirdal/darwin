namespace GameLib.Actions.Loot
{
  public class LootAllAction : Action
  {
    public static string CanonicalName => "Action.LootAll";
    public LootAllAction(string ownerId)
      : base(ownerId, name: CanonicalName, cooldownDurationMilisecs: 100)
    {
    }
  }
}