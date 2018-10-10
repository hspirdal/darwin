using RogueSharp;

namespace GameLib.Area
{
	public sealed class Cell : RogueSharp.ICell
	{
		public int X { get; set; }
		public int Y { get; set; }
		public bool IsTransparent { get; set; }
		public bool IsWalkable { get; set; }
		public bool IsInFov { get; set; }
		public bool IsExplored { get; set; }
		public CellContent Content { get; set; }

		public Cell()
		{
			Content = new CellContent();
		}

		public bool Equals(ICell other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return X == other.X && Y == other.Y && IsTransparent == other.IsTransparent &&
			IsWalkable == other.IsWalkable && IsInFov == other.IsInFov && IsExplored == other.IsExplored;
		}
	}
}