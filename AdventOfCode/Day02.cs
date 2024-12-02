using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    //[RunTest]
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
                    if (IsSafe(report))
                    {
                        numSafe++;
                        break;
                    }
                    report.Insert(i, temp);
                }
            }
            return new(numSafe.ToString());
        }

        private bool IsSafe(IEnumerable<int> report)
        {
            bool safe = true;
            bool increasing = report.ElementAt(1) > report.ElementAt(0);

            for (int i = 0; i < report.Count() - 1; i++)
            {
                if ((increasing && report.ElementAt(i + 1) <= report.ElementAt(i)) || (!increasing && report.ElementAt(i + 1) >= report.ElementAt(i)))
                {
                    safe = false;
                    break;
                }
                if (report.ElementAt(i + 1) == report.ElementAt(i) || Math.Abs(report.ElementAt(i + 1) - report.ElementAt(i)) > 3)
                {
                    safe = false;
                    break;
                }
            }
            return safe;
        }
    }
}
