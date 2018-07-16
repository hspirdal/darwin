namespace GameLib.Properties.Stats
{
	public class AbilityScores
	{
		public AttributeScore Strength { get; set; }
		public AttributeScore Dexterity { get; set; }
		public AttributeScore Constitution { get; set; }
		public AttributeScore Intelligence { get; set; }
		public AttributeScore Wisdom { get; set; }
		public AttributeScore Charisma { get; set; }

		public AbilityScores()
		{
			Strength = new AttributeScore();
			Dexterity = new AttributeScore();
			Constitution = new AttributeScore();
			Intelligence = new AttributeScore();
			Wisdom = new AttributeScore();
			Charisma = new AttributeScore();
		}

		public AbilityScores(int strength, int dexterity, int constitution, int intelligence, int wisdom, int charisma)
		{
			Strength = new AttributeScore(strength);
			Dexterity = new AttributeScore(dexterity);
			Constitution = new AttributeScore(constitution);
			Intelligence = new AttributeScore(intelligence);
			Wisdom = new AttributeScore(wisdom);
			Charisma = new AttributeScore(charisma);
		}
	}
}