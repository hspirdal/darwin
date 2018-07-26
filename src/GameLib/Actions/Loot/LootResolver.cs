using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using GameLib.Actions;
using GameLib.Area;
using GameLib.Logging;
using GameLib.Entities;

namespace GameLib.Actions.Loot
{
  public class LootResolver : IResolver
  {
    private readonly ILogger _logger;
    private readonly IFeedbackWriter _feedbackWriter;
    private readonly ICreatureRegistry _creatureRegistry;
    private readonly IPlayArea _playArea;
    public string ActionName => LootAction.CanonicalName;

    public LootResolver(ILogger logger, IFeedbackWriter feedbackWriter, ICreatureRegistry creatureRegistry, IPlayArea playArea)
    {
      _logger = logger;
      _feedbackWriter = feedbackWriter;
      _creatureRegistry = creatureRegistry;
      _playArea = playArea;
    }

    public Task ResolveAsync(Action action)
    {
      // TODO: Improve design to avoid casting.
      var lootAction = (LootAction)action;
      Resolve(lootAction.OwnerId, lootAction.ItemId);
      return Task.CompletedTask;
    }

    private void Resolve(string creatureId, string itemId)
    {
      var creature = _creatureRegistry.GetById(creatureId) as Player; // tmp cast
      var cell = _playArea.GameMap.GetCell(creature.Position.X, creature.Position.Y);
      var item = cell.Content.Entities.SingleOrDefault(i => i.Type == "Item" && i.Id == itemId) as Item;
      if (item != null)
      {
        cell.Content.Entities.Remove(item);
        creature.Inventory.Items.Add(item);
        _logger.Info($"Player '{creature.Name}' looted item '{item.Name}'");
        _feedbackWriter.WriteSuccess(creatureId, nameof(Action), $"Looted {item.Name}");
      }
      else
      {
        _logger.Warn($"Player '{creature.Name}' failed to loot item with id '{itemId}'. It was either not found, or did not cast to item.");
      }
    }
  }
}