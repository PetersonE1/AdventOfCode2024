using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    //[RunTest]
    public class Day06 : TestableDay
    {
        private readonly string _input;
        private readonly bool[,] _map;
        private readonly bool[,] _visitedMap;
        private (int x, int y) _start;

        public Day06()
        {
            _input = File.ReadAllText(InputFilePath);
            var tmp = _input.Split("\r\n").Select(n => n.Select(c => c == '#' ? 1 : c == '^' ? 2 : 0).ToArray()).ToArray();

            _map = new bool[tmp.Length, tmp[0].Length];
            _visitedMap = new bool[tmp.Length, tmp[0].Length];

            for (int y = 0; y < tmp.Length; y++)
            {
                for (int x = 0; x < tmp[0].Length; x++)
                {
                    _map[x, y] = tmp[y][x] == 1;
                    if (tmp[y][x] == 2)
                    {
                        _start.x = x;
                        _start.y = y;
                        _visitedMap[x, y] = true;
                    }
                }
            }
        }

        private enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        public override ValueTask<string> Solve_1()
        {
            (int x, int y) position = _start;
            Direction direction = Direction.Up;

            while (true)
            {
                (int x, int y) prospectivePos = direction switch
                {
                    Direction.Up => (position.x, position.y - 1),
                    Direction.Down => (position.x, position.y + 1),
                    Direction.Left => (position.x - 1, position.y),
                    Direction.Right => (position.x + 1, position.y),
                    _ => throw new InvalidOperationException()
                };
                if (prospectivePos.x < 0 || prospectivePos.x >= _map.GetLength(0) || prospectivePos.y < 0 || prospectivePos.y >= _map.GetLength(1))
                {
                    break;
                }

                if (!_map[prospectivePos.x, prospectivePos.y])
                {
                    _visitedMap[prospectivePos.x, prospectivePos.y] = true;
                    position = prospectivePos;
                }
                else
                {
                    direction = direction switch
                    {
                        Direction.Up => Direction.Right,
                        Direction.Right => Direction.Down,
                        Direction.Down => Direction.Left,
                        Direction.Left => Direction.Up,
                        _ => throw new InvalidOperationException()
                    };
                }
            }

            int numVisited = 0;
            foreach (bool visited in _visitedMap)
            {
                if (visited)
                {
                    numVisited++;
                }
            }

            return new ValueTask<string>(numVisited.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            throw new NotImplementedException();
        }

        private void PrintMap()
        {
            for (int y = 0; y < _map.GetLength(0); y++)
            {
                for (int x = 0; x < _map.GetLength(1); x++)
                {
                    Console.Write(_map[x, y] ? '#' : '.');
                }
                Console.WriteLine();
            }
        }
    }
}
