using System.Threading.Tasks;
using TcpGameServer.Actions.Movement;

namespace TcpGameServer.Actions
{
	public interface IActionResolver
	{
		Task ResolveAsync();
	}

	public class ActionResolver : IActionResolver
	{
		private readonly IActionRepository _actionRepository;
		private readonly IMovementResolver _movementResolver;

		public ActionResolver(IActionRepository actionRepository, IMovementResolver movementResolver)
		{
			this._actionRepository = actionRepository;
			this._movementResolver = movementResolver;
		}

		public async Task ResolveAsync()
		{
			var actions = _actionRepository.DequeueActions();
			var movementActions = actions.ConvertAll(list => (MovementAction)list);

			await _movementResolver.ResolveAsync(movementActions).ConfigureAwait(false);
		}
	}
}