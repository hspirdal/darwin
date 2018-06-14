using System.Collections.Generic;
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
		private readonly IReadOnlyDictionary<string, IResolver> _resolverMap;

		public ActionResolver(IActionRepository actionRepository, IDictionary<string, IResolver> resolverMap)
		{
			this._actionRepository = actionRepository;
			_resolverMap = new Dictionary<string, IResolver>(resolverMap);
		}

		public async Task ResolveAsync()
		{
			var actions = _actionRepository.PullOut();
			foreach(var action in actions)
			{
				await _resolverMap[action.Name].ResolveAsync(action).ConfigureAwait(false);
			}
		}
	}
}