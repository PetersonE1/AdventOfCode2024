using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    //[RunTest]
    public class Day07 : TestableDay
    {
        private readonly string _input;
        private (ulong result, ulong[] values)[] _equations;

        public Day07()
        {
            _input = File.ReadAllText(InputFilePath);
            _equations = InputParser(_input).ToArray();
        }

        private IEnumerable<(ulong, ulong[])> InputParser(string input)
        {
            foreach (string line in _input.Split("\r\n"))
            {
                string[] split = line.Split();
                ulong result = ulong.Parse(split[0].Trim(':'));
                ulong[] values = split.Skip(1).Select(n => ulong.Parse(n)).ToArray();
                yield return (result, values);
            }
        }

        public override ValueTask<string> Solve_1()
        {
            ulong solveableEquationsSum = 0;

            foreach ((ulong result, ulong[] values) in _equations)
            {
                if (StepValue(result, values.Skip(1).ToArray(), values[0]))
                    solveableEquationsSum += result;
            }

            return new ValueTask<string>(solveableEquationsSum.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            ulong solveableEquationsSum = 0;

            int count = 0;
            int total = _equations.Length;
            foreach ((ulong result, ulong[] values) in _equations)
            {
                if (StepValue_2(result, values.Skip(1).ToArray(), values[0]))
                {
                    solveableEquationsSum += result;
                    Console.WriteLine($"Solved {++count}/{total} equations ({((double)count / total * 100):F}%)");
                }
            }

            return new ValueTask<string>(solveableEquationsSum.ToString());
        }

        private bool StepValue(ulong result, ulong[] values, ulong current)
        {
            ulong sum = current + values[0];
            ulong product = current * values[0];

            if (values.Length == 1)
                return sum == result || product == result;

            if (sum < result)
            {
                if (StepValue(result, values.Skip(1).ToArray(), sum)) return true;
                if (StepValue(result, values.Skip(1).ToArray(), product)) return true;
            }

            if (product < result || (values.Length > 1 && values[1] == 1))
            {
                if (StepValue(result, values.Skip(1).ToArray(), sum)) return true;
                if (StepValue(result, values.Skip(1).ToArray(), product)) return true;
            }

            return false;
        }

        private bool StepValue_2(ulong result, ulong[] values, ulong current)
        {
            ulong sum = current + values[0];
            ulong product = current * values[0];
            ulong concat = ulong.Parse(current.ToString() + values[0].ToString());

            if (values.Length == 1)
                return sum == result || product == result || concat == result;

            if (sum < result || (values.Length > 1 && values[1] == 1))
            {
                if (StepValue_2(result, values.Skip(1).ToArray(), sum)) return true;
                if (StepValue_2(result, values.Skip(1).ToArray(), product)) return true;
                if (StepValue_2(result, values.Skip(1).ToArray(), concat)) return true;
            }

            if (product < result || (values.Length > 1 && values[1] == 1))
            {
                if (StepValue_2(result, values.Skip(1).ToArray(), sum)) return true;
                if (StepValue_2(result, values.Skip(1).ToArray(), product)) return true;
                if (StepValue_2(result, values.Skip(1).ToArray(), concat)) return true;
            }

            if (concat < result || (values.Length > 1 && values[1] == 1))
            {
                if (StepValue_2(result, values.Skip(1).ToArray(), sum)) return true;
                if (StepValue_2(result, values.Skip(1).ToArray(), product)) return true;
                if (StepValue_2(result, values.Skip(1).ToArray(), concat)) return true;
            }

            return false;
        }
    }
}
