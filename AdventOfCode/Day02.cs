using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    [RunTest]
    public class Day02 : TestableDay
    {
        private readonly string _input;
        private int[][] _reports;

        public Day02()
        {
            _input = File.ReadAllText(InputFilePath);
            _reports = _input.Split("\r\n").Select(n => n.Split(" ").Select(n => int.Parse(n)).ToArray()).ToArray();
        }

        public override ValueTask<string> Solve_1()
        {
            int numSafe = 0;
            foreach (int[] report in _reports)
            {
                bool safe = true;
                bool increasing = report[1] > report[0];
                for (int i = 0; i < report.Length - 1; i++)
                {
                    if ((increasing && report[i + 1] <= report[i]) || (!increasing && report[i + 1] >= report[i]))
                    {
                        safe = false;
                        break;
                    }
                    if (report[i+1] == report[i] || Math.Abs(report[i+1] - report[i]) > 3)
                    {
                        safe = false;
                        break;
                    }
                }
                if (safe)
                    numSafe++;
            }
            return new(numSafe.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            throw new NotImplementedException();
        }
    }
}
