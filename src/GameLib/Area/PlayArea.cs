
namespace GameLib.Area
{
	public interface IPlayArea
	{
		IMap GameMap { get; set; }
	}

	public class PlayArea : IPlayArea
	{
		public IMap GameMap { get; set; }
	}
}