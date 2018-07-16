using Newtonsoft.Json;
using Client.Contracts;
using GameLib.Logging;

namespace GameLib.Actions.Loot
{
	public class LootInserter : IRequestInserter
	{
		private readonly ILogger _logger;
		private readonly IActionRepository _actionRepository;

		public string ActionName => "action.loot";

		public LootInserter(ILogger logger, IActionRepository actionRepository)
		{
			_logger = logger;
			_actionRepository = actionRepository;

		}

		public void Resolve(string clientId, ClientRequest clientRequest)
		{
			var action = JsonConvert.DeserializeObject<LootAction>(clientRequest.Payload);
			if (IsValidAction(action))
			{
				action.Name = ActionName;
				action.OwnerId = clientId; // tmp
				action.ItemId = action.ItemId;
				_actionRepository.PushInto(action);
			}
		}

		private bool IsValidAction(LootAction action)
		{
			if (!action.IsValid())
			{
				_logger.Warn($"{nameof(LootAction)} did not validate correctly. Fields: {action}");
				return false;
			}
			return true;
		}
	}
}