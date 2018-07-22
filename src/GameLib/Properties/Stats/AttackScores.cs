using GameLib.Utility;

namespace GameLib.Properties.Stats
{
	public class AttackScores : IDeepCopy<AttackScores>
	{
		public AttackScores DeepCopy()
		{
			return new AttackScores();
		}
	}
}