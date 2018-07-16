namespace GameLib.Properties.Stats
{
	public class AttributeScore
	{
		public int Base { get; set; }
		public int Total => Base;

		public AttributeScore()
		{
				
		}

		public AttributeScore(int baseScore)
		{
				Base = baseScore;
		}

		public override string ToString()
		{
			return Total.ToString();
		}
	}
}