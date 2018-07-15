using Newtonsoft.Json;
using TcpGameServer.Contracts;

namespace GameLib.Actions.Loot
{
	public class LootAllInserter : IRequestInserter
	{
		private readonly IActionRepository _actionRepository;

		public string ActionName => "action.lootall";

		public LootAllInserter(IActionRepository actionRepository)
		{
			_actionRepository = actionRepository;

		}

		public void Resolve(string clientId, ClientRequest clientRequest)
		{
			var action = JsonConvert.DeserializeObject<LootAllAction>(clientRequest.Payload);
			action.Name = ActionName;
			action.OwnerId = clientId; // tmp
			_actionRepository.PushInto(action);
		}
	}
}