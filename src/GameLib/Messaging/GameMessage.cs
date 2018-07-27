namespace GameLib.Messaging
{
	public class GameMessage
	{
		public string FromEntityId { get; set; }
		public string ToEntityId { get; set; }
		public MessageTopic Topic { get; set; }

		public GameMessage(string fromEntityId, string toEntityId, MessageTopic topic)
		{
			FromEntityId = fromEntityId;
			ToEntityId = toEntityId;
			Topic = topic;
		}
	}
}