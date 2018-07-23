using System;
using System.Linq;
using GameLib.Combat;

namespace GameLib.Utility
{
	public interface IRandomGenerator
	{
		int Next(int minimum, int maximum);
		int Roll(DiceType dice, int numberOfDices);
		int D4();
		int D6();
		int D8();
		int D10();
		int D12();
		int D20();
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

		public int D20()
		{
			return _random.Next(1, 21);
		}

		public int D12()
		{
			return _random.Next(1, 13);
		}

		public int D10()
		{
			return _random.Next(1, 11);
		}

		public int D8()
		{
			return _random.Next(1, 9);
		}

		public int D6()
		{
			return _random.Next(1, 7);
		}

		public int D4()
		{
			return _random.Next(1, 5);
		}

		public int Roll(DiceType diceType, int numberOfDices)
		{
			var total = 0;
			for (var i = 0; i < numberOfDices; ++i)
			{
				total += _random.Next(1, (int)diceType);
			}
			return total;
		}
	}
}