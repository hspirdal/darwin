namespace GameLib.Messaging
{
	public class GameMessage
	{
		public string FromEntityId { get; set; }
		public string ToEntityId { get; set; }
		public MessageTopic Topic { get; set; }

		public GameMessage()
		{
			FromEntityId = string.Empty;
			ToEntityId = string.Empty;
		}

		public GameMessage(string fromEntityId, string toEntityId, MessageTopic topic)
		{
			FromEntityId = fromEntityId;
			ToEntityId = toEntityId;
			Topic = topic;
		}
	}
}