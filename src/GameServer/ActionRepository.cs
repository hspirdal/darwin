using System.Collections.Generic;

namespace GameServer
{
    public interface IActionRepository
    {
        List<IAction> GetQueuedActions();
    }

    public class ActionRepository : IActionRepository
    {
        public List<IAction> GetQueuedActions()
        {
            return new List<IAction>();
        }
    }
}