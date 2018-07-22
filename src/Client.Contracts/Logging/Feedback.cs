namespace Client.Contracts.Logging
{
	public enum FeedbackType { Information, Success, Failure }
	public class Feedback
	{
		public string Message { get; set; }
		public string Category { get; set; }
		public FeedbackType Type { get; set; }
	}
}