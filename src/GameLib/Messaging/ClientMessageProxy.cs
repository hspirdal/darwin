using System.Threading.Tasks;
using Client.Contracts;
using Newtonsoft.Json;

namespace GameLib.Messaging
{
	public class ClientMessageProxy : IMessageRecipient
	{
		private readonly IClientSender _clientSender;
		private readonly string _playerId;
		private readonly string _channel;

		public ClientMessageProxy(IClientSender clientSender, string playerId)
		{
			_clientSender = clientSender;
			_playerId = playerId;
			_channel = "gamemessage";
		}

		public string Id => _playerId;

		public void Receive(GameMessage message)
		{
			var response = new ServerResponse
			{
				Type = nameof(GameMessage),
				Message = message.Topic.ToString(),
				Payload = JsonConvert.SerializeObject(message)
			};

			Task.Run(() => _clientSender.SendAsync(_playerId, _channel, response));
		}
	}
}