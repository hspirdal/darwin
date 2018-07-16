using Newtonsoft.Json;
using Client.Contracts;
using GameLib.Logging;

namespace GameLib.Actions.Movement
{
	public class MovementInserter : IRequestInserter
	{
		private readonly ILogger _logger;
		private readonly IActionRepository _actionRepository;

		public string ActionName => "action.movement";

		public MovementInserter(ILogger logger, IActionRepository actionRepository)
		{
			_logger = logger;
			_actionRepository = actionRepository;

		}

		public void Resolve(string clientId, ClientRequest clientRequest)
		{

			var movementAction = JsonConvert.DeserializeObject<MovementAction>(clientRequest.Payload);
			movementAction.Name = ActionName;
			movementAction.OwnerId = clientId; // tmp
			_actionRepository.PushInto(movementAction);
		}

		private bool IsValidAction(MovementAction action)
		{
			if (!action.IsValid())
			{
				_logger.Warn($"{nameof(MovementAction)} did not validate correctly. Fields: {action}");
				return false;
			}
			return true;
		}
	}
}