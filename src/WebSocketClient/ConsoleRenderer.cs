using System;
using TcpGameServer.Contracts.Area;

namespace WebSocketClient
{
    public class ConsoleRenderer
    {
        public void Render(Map map, int posx, int posy)
        {
            Console.SetCursorPosition(0, 0);

            for (var y = 0; y < map.Height; ++y)
            {
                for (var x = 0; x < map.Width; ++x)
                {
                    var cell = map.GetCell(x, y);
                    var cellSymbol = cell.IsWalkable ? ' ' : '#';
                    if (posx == x && posy == y)
                    {
                        cellSymbol = '@';
                    }

                    Console.Write(cellSymbol);
                }
                Console.Write(Environment.NewLine);
            }
        }
    }
}