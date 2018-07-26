using System;
using System.Collections.Generic;

namespace GameLib.Actions.Movement
{
  public class MovementActionFactory : ActionFactory
  {
    public override string ActionName => MovementAction.CanonicalName;

    public override Action Create(string ownerId, IDictionary<string, string> parameters)
    {
      var direction = GetRequired("MovementDirection", parameters);
      var movementDirection = (MovementDirection)Enum.Parse(typeof(MovementDirection), direction);

      return new MovementAction(ownerId, movementDirection);
    }
  }
}