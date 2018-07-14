namespace GameLib.Area
{
	public interface IMapGenerator
	{
		Map Generate(int height, int width);
	}

	public class MapGenerator : IMapGenerator
	{
		public Map Generate(int width, int height)
		{
			return new Map(RogueSharp.Map.Create(new RogueSharp.MapCreation.CaveMapCreationStrategy<RogueSharp.Map>(width, height, 45, 4, 3)));
		}
	}
}