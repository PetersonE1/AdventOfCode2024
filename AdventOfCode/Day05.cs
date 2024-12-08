using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    //[RunTest]
    public class Day05 : TestableDay
    {
        private readonly string _input;
        private readonly Rule[] _rules;
        private readonly Rule_2[] _secondRules;
        private readonly int[][] _updates;
        private List<List<int>> _badUpdates = new();

        public Day05()
        {
            _input = File.ReadAllText(InputFilePath);
            var tmp = _input.Split("\r\n\r\n").Select(n => n.Split("\r\n"));
            _rules = tmp.First().Select(n => new Rule(n.Split('|').Select(num => int.Parse(num)).ToArray())).ToArray();
            _secondRules = _rules.Select(n => new Rule_2(n.RulePair)).ToArray();
            _updates = tmp.Last().Select(n => n.Split(',').Select(num => int.Parse(num)).ToArray()).ToArray();
        }

        public enum RuleResult
        {
            Good,
            OutOfOrder,
            Bad,
            Done
        }

        public class Rule
        {
            public int[] RulePair;
            public bool OutOfOrder { get; private set; }
            public bool Done => _orderQueue.Count == 0;

            private int _index;

            private Queue<int> _orderQueue;
            public Queue<int> Queue => _orderQueue;

            public Rule(int[] order)
            {
                if (order.Length != 2)
                    throw new ArgumentException("Rule must have 2 values");
                RulePair = order;

                OutOfOrder = false;

                _orderQueue = new Queue<int>(order);
            }

            public RuleResult Check(int value)
            {
                if (_orderQueue.Count == 0)
                    return RuleResult.Done;

                if (_orderQueue.Peek() == value)
                {
                    _orderQueue.Dequeue();
                    return OutOfOrder ? RuleResult.Bad : RuleResult.Good;
                }

                if (RulePair.Contains(value))
                {
                    int tmp = _orderQueue.Dequeue();
                    _orderQueue.Clear();
                    _orderQueue.Enqueue(tmp);

                    OutOfOrder = true;
                    return RuleResult.OutOfOrder;
                }
                return RuleResult.Good;
            }

            public void Reset()
            {
                _orderQueue = new Queue<int>(RulePair);
                OutOfOrder = false;
            }
        }

        public class Rule_2
        {
            public int[] RulePair;
            public int[] ItemIndices;
            public bool IsActive { get; private set; }

            public Rule_2(int[] order)
            {
                if (order.Length != 2)
                    throw new ArgumentException("Rule must have 2 values");
                RulePair = order;
            }

            public void Init(List<int> update)
            {
                ItemIndices = new int[2];
                ItemIndices[0] = update.IndexOf(RulePair[0]);
                ItemIndices[1] = update.IndexOf(RulePair[1]);
                IsActive = ItemIndices[0] != -1 && ItemIndices[1] != -1;
            }
        }

        public override ValueTask<string> Solve_1()
        {
            int sum = 0;
            foreach (int[] update in _updates)
            {
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
                else
                    _badUpdates.Add(update.ToList());

                foreach (Rule rule in _rules)
                {
                    rule.Reset();
                }
            }
            return new ValueTask<string>(sum.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            int sum = 0;

            foreach (List<int> update in _badUpdates)
            {
                foreach (Rule_2 rule in _secondRules)
                {
                    rule.Init(update);
                }

                SortUpdate(update);
                sum += update[(update.Count - 1) / 2];
            }

            CheckUnsortedUpdates();

            return new ValueTask<string>(sum.ToString());
        }

        private void SortUpdate(List<int> update)
        {
            List<int> cached_update;
            do
            {
                cached_update = new (update);
                foreach (Rule_2 rule in _secondRules.Where(_secondRules => _secondRules.IsActive))
                {
                    if (rule.ItemIndices[0] > rule.ItemIndices[1])
                    {
                        int tmp = update[rule.ItemIndices[0]];
                        update.RemoveAt(rule.ItemIndices[0]);
                        update.Insert(rule.ItemIndices[1], tmp);

                        foreach (Rule_2 rule2 in _secondRules.Where(_secondRules => _secondRules.IsActive))
                        {
                            rule2.Init(update);
                        }
                    }
                }
            }
            while (!cached_update.SequenceEqual(update));
        }

        private void CheckUnsortedUpdates(bool verbose = false)
        {
            int numBadUpdates = 0;
            foreach (List<int> update in _badUpdates)
            {
                foreach (int value in update)
                {
                    if (verbose) Console.Write($"{value} ");
                    foreach (Rule rule in _rules)
                    {
                        rule.Check(value);
                    }
                }
                if (_rules.Any(rule => rule.OutOfOrder && rule.Done))
                {
                    if (verbose) Console.Write("Bad");
                    numBadUpdates++;
                }
                foreach (Rule rule in _rules)
                {
                    rule.Reset();
                }
                if (verbose) Console.WriteLine();
            }
            Console.WriteLine($"Error Check: Detected {numBadUpdates} bad updates.");
        }

        private void PrintUpdate(List<int> update)
        {
            foreach (int value in update)
            {
                Console.Write($"{value},");
            }
            Console.WriteLine();
        }
    }
}
