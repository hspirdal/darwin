using System;
using System.Collections.Generic;
using System.Linq;
using TcpGameServer.Contracts.Area;

namespace GameLib.Area
{
	public class Map
	{
		private readonly RogueSharp.IMap _map;
		private readonly CellContent[][] _cellContent;

		public Map(RogueSharp.IMap map)
		{
			_map = map;
			_cellContent = new CellContent[map.Height][];
			for (var column = 0; column < map.Height; ++column)
			{
				_cellContent[column] = new CellContent[map.Width];
				for (var row = 0; row < map.Width; ++row)
				{
					_cellContent[column][row] = new CellContent();
				}
			}
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
			return _map.GetAllCells().Where(i => i.IsInFov).Select(c => new Cell
			{
				X = c.X,
				Y = c.Y,
				IsWalkable = c.IsWalkable,
				Content = _cellContent[c.Y][c.X]
			}).ToList();
		}

		public List<Cell> GetAllWalkableCells()
		{
			return _map.GetAllCells().Where(i => i.IsWalkable).Select(c => new Cell
			{
				X = c.X,
				Y = c.Y,
				IsWalkable = c.IsWalkable,
				Content = _cellContent[c.Y][c.X]
			}).ToList();
		}

		public Cell GetCell(int x, int y)
		{
			if (x < Width && y < Height)
			{
				var cell = _map.GetCell(x, y);
				return new Cell { X = x, Y = y, IsWalkable = cell.IsWalkable, Content = _cellContent[y][x] };
			}
			throw new ArgumentException("X or Y was out of bounds.");
		}

		public void Add(int x, int y, Entity entity)
		{
			_cellContent[y][x].Entities.Add(entity);
		}

		public void Remove(int x, int y, Entity entity)
		{
			var entities = _cellContent[y][x].Entities;
			var entityToRemove = entities.SingleOrDefault(i => i.Id == entity.Id);
			var success = entities.Remove(entityToRemove);

			if (!success)
			{
				throw new ArgumentException($"Could not remove entity of id '{entity.Id}' from position [{x}, {y}]. Entity count of cell: {entities.Count}");
			}
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