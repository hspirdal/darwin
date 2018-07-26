using System.Collections.Generic;

namespace GameLib.Actions.Combat
{
  public class AttackActionFactory : ActionFactory
  {
    public override string ActionName => AttackAction.CanonicalName;

    public override Action Create(string ownerId, IDictionary<string, string> parameters)
    {
      var targetId = GetRequired("TargetId", parameters);
      return new AttackAction(ownerId, targetId);
    }
  }
}