using System;

namespace GameLib.Utility
{
	public interface IRandomGenerator
	{
		int Next(int minimum, int maximum);
	}
	public class RandomGenerator : IRandomGenerator
	{
		private readonly Random _random;

		public RandomGenerator()
		{
			_random = new Random();
		}
		public int Next(int minimum, int maximum)
		{
			return _random.Next(minimum, maximum);
		}
	}
}