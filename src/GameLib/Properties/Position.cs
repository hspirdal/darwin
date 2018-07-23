using GameLib.Utility;

namespace GameLib.Properties
{
	public class Position : IDeepCopy<Position>
	{
		public int X { get; set; }
		public int Y { get; set; }

		public Position()
		{
		}

		public Position(int x, int y)
		{
			X = x;
			Y = y;
		}

		public void SetPosition(int x, int y)
		{
			X = x;
			Y = y;
		}

		public void Move(int x, int y)
		{
			X += x;
			Y += y;
		}

		public Position DeepCopy()
		{
			return new Position(X, Y);
		}
	}
}