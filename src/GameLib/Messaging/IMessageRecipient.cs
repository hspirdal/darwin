namespace GameLib.Messaging
{
	public interface IMessageRecipient : IIdentifiable
	{
		void Receive(GameMessage message);
	}
}