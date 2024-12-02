using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
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
                if (ProblemIndex(report) == -1)
                    numSafe++;
            }
            return new(numSafe.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            int numSafe = 0;

            foreach (int[] report_arr in _reports)
            {
                int problemIndex = ProblemIndex(report_arr);
                if (problemIndex == -1)
                {
                    numSafe++;
                    continue;
                }

                List<int> report = report_arr.ToList();
                
                foreach (int i in new int[] { 0, 1, problemIndex - 1, problemIndex })
                {
                    int temp = report[i];
                    report.RemoveAt(i);
                    if (ProblemIndex(report.ToArray()) == -1)
                    {
                        numSafe++;
                        break;
                    }
                    report.Insert(i, temp);
                }
            }
            return new(numSafe.ToString());
        }

        private int ProblemIndex(int[] report)
        {
            int problemIndex = -1;
            bool increasing = report[1] > report[0];

            for (int i = 0; i < report.Length - 1; i++)
            {
                if ((increasing && report[i + 1] <= report[i]) || (!increasing && report[i + 1] >= report[i]))
                {
                    problemIndex = i + 1;
                    break;
                }
                if (report[i + 1] == report[i] || Math.Abs(report[i + 1] - report[i]) > 3)
                {
                    problemIndex = i + 1;
                    break;
                }
            }
            return problemIndex;
        }
    }
}
