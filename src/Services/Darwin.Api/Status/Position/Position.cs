namespace Darwin.Api.Status.Position
{
	public class Position
	{
		public int X { get; set; }
		public int Y { get; set; }

		public override string ToString()
		{
			return $"[X: {X}, {Y}]";
		}
	}
}