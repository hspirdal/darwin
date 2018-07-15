using GameLib.Area;
using RogueSharp;

namespace GameLib.Tests.Area
{
	public class TestMap : GameLib.Area.Map
	{
		public TestMap(int width, int height) : base(new RogueSharp.Map(width, height))
		{
		}
	}
}