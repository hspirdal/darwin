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
		private readonly IAutonomousRegistry _autonomousRegistry;

		public MessageDispatcher(IAutonomousRegistry autonomousRegistry)
		{
			_autonomousRegistry = autonomousRegistry;
		}
		public void Dispatch(GameMessage message)
		{
			if (_autonomousRegistry.Contains(message.ToEntityId))
			{
				var receiver = _autonomousRegistry.Get(message.ToEntityId);
				receiver.Receive(message);
			}
		}
	}
}