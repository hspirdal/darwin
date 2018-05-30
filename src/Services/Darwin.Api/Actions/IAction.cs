namespace Darwin.Api.Actions
{
	public interface IAction
	{
		int OwnerId { get; }
		string Name { get; }
		void Resolve();
	}
}