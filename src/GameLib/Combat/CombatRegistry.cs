using System;
using System.Collections.Generic;
using System.Linq;
using GameLib.Logging;
using GameLib.Messaging;

namespace GameLib.Combat
{
	public interface ICombatRegistry
	{
		bool IsInCombat(string id);
		CombatEntry GetEntry(string id);
		List<CombatEntry> GetAll();
		void Register(string attackerId, string targetId);
		void Remove(string attackerId);
	}

	public class CombatRegistry : ICombatRegistry
	{
		private readonly Dictionary<string, CombatEntry> _combatMap;
		private readonly ILogger _logger;
		private readonly IMessageDispatcher _messageDispatcher;

		public CombatRegistry(ILogger logger, IMessageDispatcher messageDispatcher)
		{
			_logger = logger;
			_messageDispatcher = messageDispatcher;
			_combatMap = new Dictionary<string, CombatEntry>();
		}

		public bool IsInCombat(string id)
		{
			return _combatMap.ContainsKey(id);
		}

		public CombatEntry GetEntry(string id)
		{
			if (_combatMap.ContainsKey(id))
			{
				var success = _combatMap.TryGetValue(id, out CombatEntry combatEntry);
				if (success)
				{
					return combatEntry;
				}
			}

			throw new ArgumentException($"Was not able to retrieve entry with id '{id}'");
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

			_messageDispatcher.Dispatch(new GameMessage(attackerId, targetId, MessageTopic.AttackedBy));
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