namespace GameLib.Utility
{
	public interface IDeepCopy<out T> where T : new()
	{
		T DeepCopy();
	}
}