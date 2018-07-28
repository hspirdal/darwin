using System.Collections.Generic;
using System.Linq;

namespace GameLib.Dice
{
	public class CombinedDiceResult
	{
		public List<DiceResult> Results { get; set; }
		public int Total => Results.Sum(i => i.Result);

		public CombinedDiceResult()
		{
			Results = new List<DiceResult>();
		}
	}
}