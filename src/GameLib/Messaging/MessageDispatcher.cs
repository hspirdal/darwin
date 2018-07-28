using GameLib.Entities;
using GameLib.Properties.Autonomy;

namespace GameLib.Messaging
{
	public interface IMessageDispatcher
	{
		void Dispatch(GameMessage message);
	}

	public class MessageDispatcher : IMessageDispatcher
	{
		private readonly IRecipientRegistry _recipientRegistry;

		public MessageDispatcher(IRecipientRegistry recipientRegistry)
		{
			_recipientRegistry = recipientRegistry;
		}
		public void Dispatch(GameMessage message)
		{
			if (_recipientRegistry.Contains(message.ToEntityId))
			{
				var receiver = _recipientRegistry.Get(message.ToEntityId);
				receiver.Receive(message);
			}
		}
	}
}