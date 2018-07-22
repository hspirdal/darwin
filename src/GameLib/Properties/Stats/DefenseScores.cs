using GameLib.Utility;

namespace GameLib.Properties.Stats
{
	public class DefenseScores : IDeepCopy<DefenseScores>
	{
		public AttributeScore ArmorClass { get; set; }
		public AttributeScore HitPoints { get; set; }

		public DefenseScores()
		{
			ArmorClass = new AttributeScore();
			HitPoints = new AttributeScore();
		}

		public DefenseScores(int armorClass, int hitpoints)
		{
			ArmorClass = new AttributeScore(armorClass);
			HitPoints = new AttributeScore(hitpoints);
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