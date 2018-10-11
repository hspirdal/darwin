using System;
using GameLib.Utility;

namespace GameLib.Properties.Stats
{
	public class AttributeScore : IDeepCopy<AttributeScore>
	{
		private int _base;
		public int Total => Base;

		public AttributeScore()
		{

		}

		public AttributeScore(int baseScore)
		{
			Base = baseScore;
		}

		public int Modifier => (Total - 10) / 2;

		public int Base
		{
			get => _base;
			set
			{
				if (value < 1 || value > 30)
				{
					throw new ArgumentException("Must be within range of 1 and 30.");
				}

				_base = value;
			}
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