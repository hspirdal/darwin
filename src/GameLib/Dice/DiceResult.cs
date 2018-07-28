namespace GameLib.Dice
{
	public class DiceResult
	{
		public DiceType DiceType { get; set; }
		public int Result { get; set; }

		public DiceResult()
		{

		}

		public DiceResult(DiceType diceType, int result)
		{
			DiceType = diceType;
			Result = result;
		}
	}
}