using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    [RunTest]
    public class Day05 : TestableDay
    {
        private readonly string _input;
        private readonly Rule[] _rules;
        private readonly int[][] _updates;

        public Day05()
        {
            _input = File.ReadAllText(InputFilePath);
            var tmp = _input.Split("\r\n\r\n").Select(n => n.Split("\r\n"));
            _rules = tmp.First().Select(n => new Rule(n.Split('|').Select(num => int.Parse(num)).ToArray())).ToArray();
            _updates = tmp.Last().Select(n => n.Split(',').Select(num => int.Parse(num)).ToArray()).ToArray();
        }

        public class Rule
        {
            public int[] RulePair;
            public bool OutOfOrder { get; private set; }
            public bool Done => _orderQueue.Count == 0;

            private Queue<int> _orderQueue;

            public Rule(int[] order)
            {
                if (order.Length != 2)
                    throw new ArgumentException("Rule must have 2 values");
                RulePair = order;

                OutOfOrder = false;

                _orderQueue = new Queue<int>(order);
            }

            public void Check(int value)
            {
                if (_orderQueue.Count == 0)
                    return;

                if (_orderQueue.Peek() == value)
                {
                    _orderQueue.Dequeue();
                    return;
                }

                if (RulePair.Contains(value))
                {
                    int tmp = _orderQueue.Dequeue();
                    _orderQueue.Clear();
                    _orderQueue.Enqueue(tmp);

                    OutOfOrder = true;
                    return;
                }
            }

            public void Reset()
            {
                _orderQueue = new Queue<int>(RulePair);
                OutOfOrder = false;
            }
        }

        public override ValueTask<string> Solve_1()
        {
            int sum = 0;
            foreach (int[] update in _updates)
            {
                bool good = true;
                foreach (int value in update)
                {
                    foreach (Rule rule in _rules)
                    {
                        rule.Check(value);
                    }
                }
                if (!_rules.Any(rule => rule.OutOfOrder && rule.Done))
                {
                    sum += update[(update.Length - 1) / 2];
                }
                foreach (Rule rule in _rules)
                {
                    rule.Reset();
                }
            }
            return new ValueTask<string>(sum.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            throw new NotImplementedException();
        }
    }
}
