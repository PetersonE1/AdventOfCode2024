using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
                if (IsSafe(report))
                    numSafe++;
            }
            return new(numSafe.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            int numSafe = 0;

            foreach (int[] report_arr in _reports)
            {
                if (IsSafe(report_arr))
                {
                    numSafe++;
                    continue;
                }
                List<int> report = report_arr.ToList();
                for (int i = 0; i < report.Count; i++)
                {
                    int temp = report[i];
                    report.RemoveAt(i);
                    if (IsSafe(report.ToArray()))
                    {
                        numSafe++;
                        break;
                    }
                    report.Insert(i, temp);
                }
            }
            return new(numSafe.ToString());
        }

        private bool IsSafe(int[] report)
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
                if (report[i + 1] == report[i] || Math.Abs(report[i + 1] - report[i]) > 3)
                {
                    safe = false;
                    break;
                }
            }
            return safe;
        }
    }
}
