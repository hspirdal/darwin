using Newtonsoft.Json;
using Client.Contracts;
using GameLib.Logging;

namespace GameLib.Actions.Loot
{
	public class LootAllInserter : IRequestInserter
	{
		private readonly ILogger _logger;
		private readonly IActionRepository _actionRepository;

		public string ActionName => "action.lootall";

		public LootAllInserter(ILogger logger, IActionRepository actionRepository)
		{
			_logger = logger;
			_actionRepository = actionRepository;

		}

		public void Resolve(string clientId, ClientRequest clientRequest)
		{
			var action = JsonConvert.DeserializeObject<LootAllAction>(clientRequest.Payload);
			if (IsValid(action))
			{
				action.Name = ActionName;
				action.OwnerId = clientId; // tmp
				_actionRepository.PushInto(action);
			}
		}

		public bool IsValid(LootAllAction action)
		{
			if (!action.IsValid())
			{
				_logger.Warn($"{nameof(LootAllAction)} did not validate correctly. Fields: {action}");
				return false;
			}
			return true;
		}
	}
}