namespace GameLib.Utility
{
	public interface IDeepCopy<T> where T : new()
	{
		T DeepCopy();
	}
}