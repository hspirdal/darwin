using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using GameLib.Actions.Movement;
using System.Collections.Concurrent;
using System.Linq;
using System.Diagnostics;
using GameLib.Logging;

namespace GameLib.Actions
{
  public interface IActionRepository
  {
    List<Action> PullOut();
    void PushInto(Action action);
    int Count();
  }

  public class ActionRepository : IActionRepository
  {
    private readonly ConcurrentDictionary<string, Action> _actionMap;
    private readonly ILogger _logger;

    public ActionRepository(ILogger logger)
    {
      _actionMap = new ConcurrentDictionary<string, Action>();
      _logger = logger;
    }

    public List<Action> PullOut()
    {
      var actions = _actionMap.Values.ToList();
      foreach (var action in actions)
      {
        _actionMap.TryRemove(action.OwnerId, out Action a);
      }

      return actions;
    }

    public void PushInto(Action action)
    {
      // TODO: With cooldown added, this constraint is not needed anymore, but may as well be in place until usecase
      // supports having multiple actions pending at once for single user (does not want to silently replace).
      var actionAlreadyStored = _actionMap.ContainsKey(action.OwnerId);
      if (actionAlreadyStored == false)
      {
        var ableToAdd = _actionMap.TryAdd(action.OwnerId, action);
        if (ableToAdd == false)
        {
          throw new InvalidOperationException($"Was not able to add action with id {action.OwnerId}");
        }
        _logger.Debug($"Append action with name '{action.Name}'");
      }
    }

    public int Count()
    {
      return _actionMap.Count();
    }
  }
}