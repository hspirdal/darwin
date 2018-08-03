using System;
using System.Threading.Tasks;
using Client.Contracts;
using GameLib.Logging;
using Newtonsoft.Json;

namespace GameLib.Messaging
{
	public class ClientMessageProxy : IMessageRecipient
	{
		private readonly IClientSender _clientSender;
		private readonly string _playerId;
		private readonly string _channel;
		private int NextSequenceNumber = 0;

		public ClientMessageProxy(IClientSender clientSender, string playerId)
		{
			_clientSender = clientSender;
			_playerId = playerId;
			_channel = "gamemessage";
		}

		public string Id => _playerId;

		public void Receive(GameMessage message)
		{
			var sequenceNumber = ++NextSequenceNumber;
			var clientMessage = new ClientMessage(message, sequenceNumber);
			var response = new ServerResponse
			{
				Type = nameof(GameMessage),
				Message = clientMessage.Topic.ToString(),
				Payload = JsonConvert.SerializeObject(clientMessage)
			};


			Console.WriteLine(clientMessage.Topic.ToString() + " seq: " + clientMessage.SequenceNumber);
			Task.Run(() =>
			{
				_clientSender.SendAsync(_playerId, _channel, response).Wait();
				Console.WriteLine("Sent " + clientMessage.Topic.ToString() + " seq: " + clientMessage.SequenceNumber);
			});
		}

		public class ClientMessage : GameMessage
		{
			public ClientMessage(GameMessage message, int sequenceNumber)
				: base(message.FromEntityId, message.ToEntityId, message.Topic, message.Payload)
			{
				SequenceNumber = sequenceNumber;
			}
			public int SequenceNumber { get; set; }
		}
	}
}