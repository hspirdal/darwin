namespace GameLib.Properties.Stats
{
	public class CharacterStatistics : Statistics
	{
		public string Race { get; set; }
		public string Class { get; set; }
		public AttributeScore Level { get; set; }

		public CharacterStatistics() : base()
		{
			Race = string.Empty;
			Class = string.Empty;
			Level = new AttributeScore();
		}
		public CharacterStatistics(string race, string charclass, int level) : base()
		{
			Race = race;
			Class = charclass;
			Level = new AttributeScore(level);
		}
	}
}