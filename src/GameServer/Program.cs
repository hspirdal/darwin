using System;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var actionRepository = new ActionRepository();

            // who needs precision or quickness >_>
            var nextGameTick = DateTime.UtcNow;
            while(true)
            {
                var currentTime = DateTime.UtcNow;
                if(currentTime > nextGameTick)
                {
                    var diff = currentTime - nextGameTick;
                    nextGameTick = DateTime.UtcNow.AddSeconds(1) - diff;
                    
                    var actions = actionRepository.GetQueuedActions();
                    foreach(var action in actions)
                    {
                        action.Resolve();
                    }
                }
            }
        }
    }
}
