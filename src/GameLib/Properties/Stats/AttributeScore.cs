using GameLib.Utility;

namespace GameLib.Properties.Stats
{
	public class AttributeScore : IDeepCopy<AttributeScore>
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

		public AttributeScore DeepCopy()
		{
			return new AttributeScore(Base);
		}
	}
}