using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    //[RunTest]
    public class Day08 : TestableDay
    {
        private readonly string _input;
        private readonly Dictionary<char, List<(int x, int y)>> _antennae;
        private readonly (int width, int height) _gridSize;

        public Day08()
        {
            _input = File.ReadAllText(InputFilePath);
            _antennae = new Dictionary<char, List<(int x, int y)>>();
            string[] lines = _input.Split("\r\n");
            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[0].Length; x++)
                {
                    char c = lines[y][x];
                    if (c == '.')
                        continue;
                    if (!_antennae.ContainsKey(c))
                        _antennae[c] = new List<(int x, int y)>();
                    _antennae[c].Add((x, y));
                }
            }
            _gridSize = (lines[0].Length, lines.Length);
        }

        public override ValueTask<string> Solve_1()
        {
            HashSet<(int x, int y)> antinodePositions = new();

            foreach (var antennae in _antennae)
            {
                CalculateAntinodes(antennae.Value).ToList().ForEach(n => { if (IsInBounds(n)) antinodePositions.Add(n); });
            }
            
            return new ValueTask<string>(antinodePositions.Count.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            throw new NotImplementedException();
        }

        private int CalculateRawAntinodeCount(int numberOfAntennae)
        {
            int denominator = Enumerable.Range(1, numberOfAntennae - 2).Aggregate(1, (n, item) => n * item);
            int numerator = Enumerable.Range(numberOfAntennae - 1, 2).Aggregate(denominator, (n, item) => n * item);
            return numerator / denominator;
        }

        private bool IsInBounds((int x, int y) position) => position.x >= 0 && position.x < _gridSize.width && position.y >= 0 && position.y < _gridSize.height;

        private HashSet<(int x, int y)> CalculateAntinodes(List<(int x, int y)> antennae)
        {
            HashSet<(int x, int y)> antinodes = new();

            for (int i = 0; i < antennae.Count; i++)
            {
                for (int j = 0; j < antennae.Count; j++)
                {
                    if (i == j)
                        continue;
                    (int dx, int dy) = (antennae[j].x - antennae[i].x, antennae[j].y - antennae[i].y);
                    antinodes.Add((antennae[j].x + dx, antennae[j].y + dy));
                }
            }
            return antinodes;
        }
    }
}
