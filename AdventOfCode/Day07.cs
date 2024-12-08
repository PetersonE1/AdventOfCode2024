using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    [RunTest]
    public class Day07 : TestableDay
    {
        private readonly string _input;
        private (int result, int[] values)[] _equations;

        public Day07()
        {
            _input = File.ReadAllText(InputFilePath);
            _equations = InputParser(_input).ToArray();
        }

        private IEnumerable<(int, int[])> InputParser(string input)
        {
            foreach (string line in _input.Split("\r\n"))
            {
                string[] split = line.Split();
                int result = int.Parse(split[0].Trim(':'));
                int[] values = split.Skip(1).Select(n => int.Parse(n)).ToArray();
                yield return (result, values);
            }
        }

        public override ValueTask<string> Solve_1()
        {
            throw new NotImplementedException();
        }

        public override ValueTask<string> Solve_2()
        {
            throw new NotImplementedException();
        }
    }
}
