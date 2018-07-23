using GameLib.Utility;

namespace GameLib.Properties.Stats
{
	public class HitPoints : IDeepCopy<HitPoints>
	{
		public int Base { get; set; }
		public int Current { get; set; }

		public HitPoints()
		{

		}

		public HitPoints(int hitpoints)
		{
			Base = hitpoints;
			Current = Base;
		}

		public int Max => Base;

		public HitPoints DeepCopy()
		{
			return new HitPoints { Base = Base, Current = Current };
		}
	}
}