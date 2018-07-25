using Newtonsoft.Json;
using Client.Contracts;
using GameLib.Logging;
using System;

namespace GameLib.Actions.Combat
{
	public class AttackInserter : IRequestInserter
	{
		private readonly ILogger _logger;
		private readonly IActionRepository _actionRepository;
		private readonly ICooldownRegistry _cooldownRegistry;

		public string ActionName => "action.attack";

		public AttackInserter(ILogger logger, IActionRepository actionRepository, ICooldownRegistry cooldownRegistry)
		{
			_logger = logger;
			_actionRepository = actionRepository;
			_cooldownRegistry = cooldownRegistry;
		}

		public void Resolve(string clientId, ClientRequest clientRequest)
		{
			var action = JsonConvert.DeserializeObject<AttackAction>(clientRequest.Payload);
			var isCooldownInEffect = _cooldownRegistry.IsCooldownInEffect(clientId);
			if (isCooldownInEffect)
			{
				_logger.Warn($"Cooldown for action '{action.Name}'. Client id '{clientId}'");
				return;
			}

			if (IsValidAction(action))
			{
				var validTo = DateTime.UtcNow + TimeSpan.FromMilliseconds(1000);
				var cooldown = new Cooldown(action.OwnerId, validTo);
				if (_cooldownRegistry.Add(cooldown))
				{
					action.Name = ActionName;
					action.OwnerId = clientId; // tmp
					_actionRepository.PushInto(action);
				}
			}
		}

		private bool IsValidAction(AttackAction action)
		{
			if (!action.IsValid())
			{
				_logger.Warn($"{nameof(AttackAction)} did not validate correctly. Fields: {action}");
				return false;
			}
			return true;
		}
	}
}