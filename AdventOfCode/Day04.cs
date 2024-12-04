using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    //[RunTest]
    public class Day04 : TestableDay
    {
        private readonly string _input;
        private readonly char[,] _board;
        private readonly char[] _xmas = ['X', 'M', 'A', 'S'];

        public Day04()
        {
            _input = File.ReadAllText(InputFilePath);

            string[] lines = _input.Split("\r\n");
            _board = new char[lines[0].Length, lines.Length];

            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[0].Length; x++)
                {
                    _board[x, y] = lines[x][y];
                }
            }
        }

        public override ValueTask<string> Solve_1()
        {
            int count = 0;

            for (int y = 0; y < _board.GetLength(1); y++)
            {
                for (int x = 0; x < _board.GetLength(0); x++)
                {
                    if (_board[x, y] != 'X')
                        continue;

                    count += AllXs(x, y);
                }
            }

            return new(count.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            int count = 0;

            for (int y = 0; y < _board.GetLength(1); y++)
            {
                for (int x = 0; x < _board.GetLength(0); x++)
                {
                    if (_board[x, y] != 'A')
                        continue;

                    if (CheckX_Mas(x, y))
                        count++;
                }
            }

            return new(count.ToString());
        }

        private bool TryGetBoard(int x, int y, out char c)
        {
            c = (char)0;
            if (x < 0 || x >= _board.GetLength(0) || y < 0 || y >= _board.GetLength(1))
                return false;

            c = _board[x, y];
            return true;
        }

        private int AllXs(int x, int y)
        {
            int count = 0;
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0)
                        continue;
                    if (CheckXmas(x, y, dx, dy, new Queue<char>(_xmas)))
                        count++;
                }
            }
            return count;
        }

        private bool CheckXmas(int x, int y, int dx, int dy, Queue<char> queue)
        {
            if (queue.Count == 0)
                return true;

            if (TryGetBoard(x, y, out char c))
            {
                if (c == queue.Dequeue())
                    return CheckXmas(x + dx, y + dy, dx, dy, queue);
            }

            return false;
        }

        private bool CheckX_Mas(int x, int y)
        {
            List<char> BackSlash = new() { 'M', 'S' };
            List<char> ForwardSlash = new() { 'M', 'S' };

            if (TryGetBoard(x - 1, y - 1, out char c) && BackSlash.Contains(c))
            {
                BackSlash.Remove(c);
                if (!(TryGetBoard(x + 1, y + 1, out c) && c == BackSlash.Single()))
                    return false;

                if (TryGetBoard(x - 1, y + 1, out c) && ForwardSlash.Contains(c))
                {
                    ForwardSlash.Remove(c);
                    if (TryGetBoard(x + 1, y - 1, out c) && c == ForwardSlash.Single())
                        return true;
                }
                return false;
            }
            return false;
        }
    }
}
