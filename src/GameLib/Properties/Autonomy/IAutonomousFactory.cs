using GameLib.Actions;
using GameLib.Entities;
using GameLib.Logging;

namespace GameLib.Properties.Autonomy
{
	public interface IAutonomousFactory
	{
		IAutonomous Create(string model, Creature creature);
	}

	public class AutonomousFactory : IAutonomousFactory
	{
		private readonly ILogger _logger;
		private readonly IActionInserter _actionInserter;

		public AutonomousFactory(ILogger logger, IActionInserter actionInserter)
		{
			_logger = logger;
			_actionInserter = actionInserter;
		}

		public IAutonomous Create(string model, Creature creature)
		{
			return new SimpleDefender(_logger, _actionInserter, creature);
		}
	}
}