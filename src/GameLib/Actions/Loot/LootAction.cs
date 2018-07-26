namespace GameLib.Actions.Loot
{
  public class LootAction : Action
  {
    public static string CanonicalName => "Action.Loot";
    public string ItemId { get; set; }

    public LootAction(string ownerId, string itemId)
      : base(ownerId, name: CanonicalName, cooldownDurationMilisecs: 100)
    {
      ItemId = itemId;
    }

    public override string ToString()
    {
      return base.ToString() + $", {nameof(ItemId)}: '{ItemId}'";
    }
  }
}