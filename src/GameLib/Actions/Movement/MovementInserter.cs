using Newtonsoft.Json;
using Client.Contracts;

namespace GameLib.Actions.Movement
{
	public class MovementInserter : IRequestInserter
	{
		private readonly IActionRepository _actionRepository;

		public string ActionName => "action.movement";

		public MovementInserter(IActionRepository actionRepository)
		{
			_actionRepository = actionRepository;

		}

		public void Resolve(string clientId, ClientRequest clientRequest)
		{
			var movementAction = JsonConvert.DeserializeObject<MovementAction>(clientRequest.Payload);
			movementAction.Name = ActionName;
			movementAction.OwnerId = clientId; // tmp
			_actionRepository.PushInto(movementAction);
		}
	}
}