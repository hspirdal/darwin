
using TcpGameServer.Contracts.Area;

namespace TcpGameServer.Area
{
	public interface IMapGenerator
	{
		Map Generate(int height, int width);
	}

	public class MapGenerator : IMapGenerator
	{
		public Map Generate(int height, int width)
		{
			var caveMap = new TcpGameServer.Contracts.Area.Map(width, height);
			var map = RogueSharp.Map.Create(new RogueSharp.MapCreation.CaveMapCreationStrategy<RogueSharp.Map>(width, height, 45, 4, 3));
			for (var y = 0; y < map.Height; ++y)
			{
				for (var x = 0; x < map.Width; ++x)
				{
					var cell = map.GetCell(x, y);
					caveMap.Cells[y, x] = new Cell
					{
						X = x,
						Y = y,
						IsWalkable = cell.IsWalkable
					};
				}
			}

			return caveMap;
		}
	}
}