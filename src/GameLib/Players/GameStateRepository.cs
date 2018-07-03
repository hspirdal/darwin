using System.Collections.Generic;
using GameLib.Area;

namespace GameLib.Players
{
    public interface IGameStateRepository
    {
        Map GetMapById(string playerId);
        void SetMapById(string playerId, Map map);
    }

    public class GameStateRepository : IGameStateRepository
    {
        private Dictionary<string, Map> _map;

        public GameStateRepository()
        {
            _map = new Dictionary<string, Map>();
        }

        public Map GetMapById(string playerId)
        {
            return _map[playerId];
        }

        public void SetMapById(string playerId, Map map)
        {
            if (!_map.ContainsKey(playerId))
            {
                _map.Add(playerId, map);
                return;
            }
            _map[playerId] = map;
        }
    }
}