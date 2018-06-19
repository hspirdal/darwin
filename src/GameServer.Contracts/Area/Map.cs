using System.Collections.Generic;

namespace TcpGameServer.Contracts.Area
{
	public class Map
	{
		public Cell[,] Cells { get; set; }

		public int Width { get; set; }
		public int Height { get; set; }

		public Map(int width, int height)
		{
			Width = width;
			Height = height;
			Cells = new Cell[height, width];
		}

		public Cell GetCell(int x, int y)
		{
			if (y < Height && x < Width)
			{
				return Cells[y, x];
			}
			return null;
		}
	}
}