namespace GameLib.Messaging
{
	public interface IMessageReceiver
	{
		void Receive(GameMessage message);
	}
}