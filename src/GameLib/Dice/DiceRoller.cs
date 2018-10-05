using GameLib.Utility;

namespace GameLib.Dice
{
	public interface IDiceRoller
	{

		CombinedDiceResult Roll(DiceType diceType, int numberOfDices);
		DiceResult D4();
		DiceResult D6();
		DiceResult D8();
		DiceResult D10();
		DiceResult D12();
		DiceResult D20();
	}

	public class DiceRoller : IDiceRoller
	{
		private readonly IRandomNumberGenerator _random;

		public DiceRoller(IRandomNumberGenerator randomNumberGenerator)
		{
			_random = randomNumberGenerator;
		}

		public DiceResult D20()
		{
			return new DiceResult(DiceType.d20, _random.Next(1, 21));
		}

		public DiceResult D12()
		{
			return new DiceResult(DiceType.d12, _random.Next(1, 13));
		}

		public DiceResult D10()
		{
			return new DiceResult(DiceType.d10, _random.Next(1, 11));
		}

		public DiceResult D8()
		{
			return new DiceResult(DiceType.d8, _random.Next(1, 9));
		}

		public DiceResult D6()
		{
			return new DiceResult(DiceType.d6, _random.Next(1, 7));
		}

		public DiceResult D4()
		{
			return new DiceResult(DiceType.d4, _random.Next(1, 5));
		}

		public CombinedDiceResult Roll(DiceType diceType, int numberOfDices)
		{
			var result = new CombinedDiceResult();
			for (var i = 0; i < numberOfDices; ++i)
			{
				result.Results.Add(new DiceResult(diceType, _random.Next(1, (int)diceType)));
			}
			return result;
		}
	}
}