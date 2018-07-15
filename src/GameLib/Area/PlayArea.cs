
namespace GameLib.Area
{
	public interface IPlayArea
	{
		Map GameMap { get; set; }
	}

	public class PlayArea : IPlayArea
	{
		public Map GameMap { get; set; }
	}
}