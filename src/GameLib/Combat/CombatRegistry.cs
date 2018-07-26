using System.Collections.Generic;
using System.Linq;
using GameLib.Logging;

namespace GameLib.Combat
{
  public interface ICombatRegistry
  {
    bool IsInCombat(string id);
    List<CombatEntry> GetAll();
    void Register(string attackerId, string targetId);
    void Remove(string attackerId);
  }

  public class CombatRegistry : ICombatRegistry
  {
    private readonly Dictionary<string, CombatEntry> _combatMap;
    private readonly ILogger _logger;

    public CombatRegistry(ILogger logger)
    {
      _logger = logger;
      _combatMap = new Dictionary<string, CombatEntry>();
    }

    public bool IsInCombat(string id)
    {
      return _combatMap.ContainsKey(id);
    }

    public List<CombatEntry> GetAll()
    {
      return _combatMap.Values.ToList();
    }

    public void Register(string attackerId, string targetId)
    {
      if (_combatMap.ContainsKey(attackerId) == false)
      {
        _combatMap.Add(attackerId, new CombatEntry { AttackerId = attackerId, TargetId = targetId });
      }
      else
      {
        _combatMap[attackerId] = new CombatEntry { AttackerId = attackerId, TargetId = targetId };
      }
    }

    public void Remove(string attackerId)
    {
      if (_combatMap.ContainsKey(attackerId))
      {
        _combatMap.Remove(attackerId);
        return;
      }

      _logger.Warn($"Could not remove creature with id '{attackerId}' as it was not found in registry.");
    }
  }
}