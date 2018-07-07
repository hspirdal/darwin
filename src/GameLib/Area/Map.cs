using System;
using System.Collections.Generic;
using System.Linq;
using TcpGameServer.Contracts.Area;

namespace GameLib.Area
{
    public class Map
    {
        private readonly RogueSharp.IMap _map;

        public Map(RogueSharp.IMap map)
        {
            _map = map;
        }

        public int Width => _map.Width;
        public int Height => _map.Height;

        public bool IsWalkable(int x, int y)
        {
            return _map.IsWalkable(x, y);
        }

        public bool IsInFov(int x, int y)
        {
            return _map.IsInFov(x, y);
        }

        public List<Cell> GetVisibleCells()
        {
            return _map.GetAllCells().Where(i => i.IsInFov).Select(c => new Cell { X = c.X, Y = c.Y, IsWalkable = c.IsWalkable }).ToList();
        }

        public List<Cell> GetAllWalkableCells()
        {
            return _map.GetAllCells().Where(i => i.IsWalkable).Select(c => new Cell { X = c.X, Y = c.Y, IsWalkable = c.IsWalkable }).ToList();
        }

        public Cell GetCell(int x, int y)
        {
            if (x < Width && y < Height)
            {
                var cell = _map.GetCell(x, y);
                return new Cell { X = x, Y = y, IsWalkable = cell.IsWalkable };
            }
            throw new ArgumentException("X or Y was out of bounds.");
        }

        public void ComputeFov(int x, int y, int radius)
        {
            _map.ComputeFov(x, y, radius, true);
        }

        public Map Clone()
        {
            return new Map(_map.Clone());
        }

    }
}