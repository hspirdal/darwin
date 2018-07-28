using GameLib.Messaging;

namespace GameLib.Properties.Autonomy
{
	public interface IAutonomous : IMessageRecipient
	{
		void Act();
	}
}