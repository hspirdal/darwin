using GameLib.Utility;

namespace GameLib.Properties.Stats
{
	public class Statistics : IDeepCopy<Statistics>
	{
		public AbilityScores AbilityScores { get; set; }
		public AttackScores AttackScores { get; set; }
		public DefenseScores DefenseScores { get; set; }

		public Statistics()
		{
			AbilityScores = new AbilityScores();
			AttackScores = new AttackScores();
			DefenseScores = new DefenseScores();
		}

		public Statistics DeepCopy()
		{
			return new Statistics
			{
				AbilityScores = AbilityScores.DeepCopy(),
				AttackScores = AttackScores.DeepCopy(),
				DefenseScores = DefenseScores.DeepCopy()
			};
		}
	}
}