using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameLib.Properties;
using StackExchange.Redis;

namespace GameLib.Users
{
	public interface IUserRepository
	{
		Task<User> GetByIdAsync(string userId);
		Task<User> GetByIdOrDefaultAsync(string userId);
		Task<List<User>> GetAllAsync();
		Task<List<User>> GetActiveAsync();
		Task<bool> ContainsAsync(string userId);
		Task AddOrUpdateAsync(User user);
		Task RemoveAsync(string userId);

	}

	public class UserRepository : AbstractRedisRepository<User>, IUserRepository
	{
		public UserRepository(IConnectionMultiplexer connectionMultiplexer)
			: base(connectionMultiplexer, "users")
		{
			// Clear user state from previous runs.
			connectionMultiplexer.GetDatabase().KeyDelete(_partitionKey);
		}

		public async Task<List<User>> GetActiveAsync()
		{
			var allUsers = await GetAllAsync().ConfigureAwait(false);
			return allUsers.Where(i => i.GameState == GameState.InGame).ToList();
		}
	}
}