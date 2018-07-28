using System;
using System.Linq;

namespace GameLib.Utility
{
	public interface IRandomNumberGenerator
	{
		int Next(int minimum, int maximum);
	}
	public class RandomNumberGenerator : IRandomNumberGenerator
	{
		private readonly Random _random;

		public RandomNumberGenerator()
		{
			_random = new Random();
		}
		public int Next(int minimum, int maximum)
		{
			return _random.Next(minimum, maximum);
		}
	}
}