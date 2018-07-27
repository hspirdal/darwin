using GameLib.Messaging;

namespace GameLib.Properties.Autonomy
{
	public interface IAutonomous : IMessageReceiver
	{
		void Act();
		string Id { get; }
	}
}