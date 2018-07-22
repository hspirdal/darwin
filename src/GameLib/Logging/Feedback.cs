namespace GameLib.Logging
{
	public enum FeedbackType { Information, Success, Failure }
	public class Feedback
	{
		public string Message { get; set; }
		public string Category { get; set; }
		public FeedbackType Type { get; set; }

		public override string ToString()
		{
			return $"Category: {Category} | Type: {Type} | Message: {Message}";
		}
	}
}