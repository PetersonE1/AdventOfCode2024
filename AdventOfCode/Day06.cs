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
            int blockableSpots = 0;

            for (int y = 0; y < _visitedMap.GetLength(0); y++)
            {
                for (int x = 0; x < _visitedMap.GetLength(1); x++)
                {
                    if (_visitedMap[x, y] && !(x == _start.x && y == _start.y))
                    {
                        bool[,] newMap = CopyMap(_map);
                        newMap[x, y] = true;

                        (int x, int y) position = _start;
                        Direction direction = Direction.Up;

                        int iter = 0;
                        while (true)
                        {
                            iter++;
                            (int x, int y) prospectivePos = direction switch
                            {
                                Direction.Up => (position.x, position.y - 1),
                                Direction.Down => (position.x, position.y + 1),
                                Direction.Left => (position.x - 1, position.y),
                                Direction.Right => (position.x + 1, position.y),
                                _ => throw new InvalidOperationException()
                            };
                            if (prospectivePos.x < 0 || prospectivePos.x >= newMap.GetLength(0) || prospectivePos.y < 0 || prospectivePos.y >= newMap.GetLength(1))
                            {
                                break;
                            }

                            if (!newMap[prospectivePos.x, prospectivePos.y])
                            {
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

                            if (iter > 50000)
                            {
                                blockableSpots++;
                                break;
                            }
                        }
                    }
                }
            }

            return new ValueTask<string>(blockableSpots.ToString());
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

        public bool[,] CopyMap(bool[,] arr)
        {
            bool[,] newArr = new bool[arr.GetLength(0), arr.GetLength(1)];
            for (int y = 0; y < arr.GetLength(1); y++)
            {
                for (int x = 0; x < arr.GetLength(0); x++)
                {
                    newArr[x, y] = arr[x, y];
                }
            }
            return newArr;
        }
    }
}
