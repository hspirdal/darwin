using GameLib.Utility;

namespace GameLib.Properties.Stats
{
	public class DefenseScores : IDeepCopy<DefenseScores>
	{
		public AttributeScore ArmorClass { get; set; }
		public HitPoints HitPoints { get; set; }

		public DefenseScores()
		{
			ArmorClass = new AttributeScore();
			HitPoints = new HitPoints();
		}

		public DefenseScores(int armorClass, int hitpoints)
		{
			ArmorClass = new AttributeScore(armorClass);
			HitPoints = new HitPoints(hitpoints);
		}

		public DefenseScores DeepCopy()
		{
			return new DefenseScores
			{
				ArmorClass = ArmorClass.DeepCopy(),
				HitPoints = HitPoints.DeepCopy()
			};
		}
	}
}