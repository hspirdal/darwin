using System.Collections.Generic;

namespace GameLib.Actions.Loot
{
  public class LootActionFactory : ActionFactory
  {
    public override string ActionName => LootAction.CanonicalName;

    public override Action Create(string ownerId, IDictionary<string, string> parameters)
    {
      var itemId = GetRequired("ItemId", parameters);
      return new LootAction(ownerId, itemId);
    }
  }
}