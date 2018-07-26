using System.Collections.Generic;

namespace GameLib.Actions.Loot
{
  public class LootAllActionFactory : ActionFactory
  {
    public override string ActionName => LootAllAction.CanonicalName;

    public override Action Create(string ownerId, IDictionary<string, string> parameters)
    {
      return new LootAllAction(ownerId);
    }
  }
}