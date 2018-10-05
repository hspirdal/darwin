namespace GameLib.Area
{
	public interface IMapGenerator
	{
		IMap Generate(int width, int height);
	}

	public class MapGenerator : IMapGenerator
	{
		public IMap Generate(int width, int height)
		{
			return new Map(RogueSharp.Map.Create(new RogueSharp.MapCreation.CaveMapCreationStrategy<RogueSharp.Map>(width, height, 45, 4, 3)));
		}
	}
}