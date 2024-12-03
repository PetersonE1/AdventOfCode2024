using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Day03 : TestableDay
    {
        private readonly string _input;
        Regex _mulRegex = new Regex(@"mul\((\d+),(\d+)\)");
        Regex _fullRegex = new Regex(@"mul\((\d+),(\d+)\)|do\(\)|don't\(\)");

        public Day03()
        {
            _input = File.ReadAllText(InputFilePath);
        }

        public override ValueTask<string> Solve_1()
        {
            int sum = 0;
            foreach (Match match in _mulRegex.Matches(_input))
            {
                sum += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
            }
            return new(sum.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            int sum = 0;
            bool doMult = true;
            foreach (Match match in _fullRegex.Matches(_input))
            {
                if (match.Groups[0].Value == "do()")
                {
                    doMult = true;
                    continue;
                }
                if (match.Groups[0].Value == "don't()")
                {
                    doMult = false;
                    continue;
                }
                if (doMult)
                    sum += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
            }
            return new(sum.ToString());
        }
    }
}
