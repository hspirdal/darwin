namespace GameLib.Properties.Stats
{
	public class DefenseScores
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
	}
}