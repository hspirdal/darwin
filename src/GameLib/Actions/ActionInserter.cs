using System;
using System.Collections.Generic;
using GameLib.Logging;
using GameLib.Utility;

namespace GameLib.Actions
{
  public interface IActionInserter
  {
    ActionInserterResponse Insert(string ownerId, string actionName, string actionPayload);
  }

  public class ActionInserter : IActionInserter
  {
    private readonly IActionFactoryRegistry _actionFactoryRegistry;
    private readonly IActionRepository _actionRepository;
    private readonly ICooldownRegistry _cooldownRegistry;
    private readonly ILogger _logger;
    private readonly IClock _clock;

    public ActionInserter(IActionFactoryRegistry actionFactoryRegistry, IActionRepository actionRepository,
      ICooldownRegistry cooldownRegistry, ILogger logger, IClock clock)
    {
      _actionFactoryRegistry = actionFactoryRegistry;
      _actionRepository = actionRepository;
      _cooldownRegistry = cooldownRegistry;
      _logger = logger;
      _clock = clock;
    }

    public ActionInserterResponse Insert(string ownerId, string actionName, string actionPayload)
    {
      if (_cooldownRegistry.IsCooldownInEffect(ownerId))
      {
        _logger.Warn($"User with id '{ownerId}' tried inserting action '{actionName}' before cooldown was over");
        return new ActionInserterResponse { Success = false };
      }

      var action = _actionFactoryRegistry.Create(ownerId, actionName, actionPayload);
      var duration = _clock.UtcNow + TimeSpan.FromMilliseconds(action.CooldownDurationMilisecs);
      var cooldown = new Cooldown(ownerId, duration);
      if (_cooldownRegistry.Add(cooldown))
      {
        _actionRepository.PushInto(action);
        return new ActionInserterResponse { Success = true, NextActionAvailableUtc = duration };
      }
      else
      {
        _logger.Error($"Failed to insert cooldown for user id '{ownerId} with action '{actionName}'");
      }

      return new ActionInserterResponse { Success = false };
    }
  }

  public class ActionInserterResponse
  {
    public bool Success { get; set; }
    public DateTime NextActionAvailableUtc { get; set; }
  }
}