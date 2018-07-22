using Newtonsoft.Json;
using Client.Contracts;
using GameLib.Logging;

namespace GameLib.Actions.Combat
{
	public class AttackInserter : IRequestInserter
	{
		private readonly ILogger _logger;
		private readonly IActionRepository _actionRepository;

		public string ActionName => "action.attack";

		public AttackInserter(ILogger logger, IActionRepository actionRepository)
		{
			_logger = logger;
			_actionRepository = actionRepository;

		}

		public void Resolve(string clientId, ClientRequest clientRequest)
		{
			var action = JsonConvert.DeserializeObject<AttackAction>(clientRequest.Payload);
			if (IsValidAction(action))
			{
				action.Name = ActionName;
				action.OwnerId = clientId; // tmp
				_actionRepository.PushInto(action);
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