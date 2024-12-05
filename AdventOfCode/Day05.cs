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
        private List<List<int>> _badUpdates = new();

        public Day05()
        {
            _input = File.ReadAllText(InputFilePath);
            var tmp = _input.Split("\r\n\r\n").Select(n => n.Split("\r\n"));
            _rules = tmp.First().Select(n => new Rule(n.Split('|').Select(num => int.Parse(num)).ToArray())).ToArray();
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
                Dictionary<Rule, int> storedIndices = new();

                for (int i = 0; i < update.Count; i++)
                {
                    int value = update[i];
                    int itemIndex = i;
                    int errorOffset = 0;
                    foreach (Rule rule in _rules)
                    {
                        RuleResult result = rule.Check(value);
                        if (result == RuleResult.OutOfOrder)
                        {
                            storedIndices.Add(rule, i - errorOffset);
                            //Console.WriteLine($"OOO: Stored {rule.RulePair[0]} and {rule.RulePair[1]} at {i} | HASH={rule.GetHashCode()}");
                        }
                        if (result == RuleResult.Bad)
                        {
                            errorOffset++;
                            //Console.WriteLine("BEFORE");
                            //foreach (var pair in storedIndices)
                                //Console.WriteLine($"{i} - Reference for {(pair.Key.Queue.Count == 0 ? "NULL" : pair.Key.Queue.Peek())} at index {pair.Value}");
                            //Console.WriteLine($"BAD: {value} out of order at {i} | HASH={rule.GetHashCode()}");
                            //Console.WriteLine($"Moving {value} from index {i} to index {storedIndices[rule]}");
                            update.RemoveAt(itemIndex);
                            update.Insert(storedIndices[rule], value);
                            itemIndex = storedIndices[rule];
                            int tmp_index = storedIndices[rule];
                            storedIndices.Remove(rule);
                            for (int j = 0; j < storedIndices.Count; j++)
                            {
                                var pair = storedIndices.ElementAt(j);
                                if (storedIndices[pair.Key] == i)
                                {
                                    //Console.WriteLine($"Relocating reference from index {i} to {tmp_index}");
                                    storedIndices[pair.Key] = tmp_index;
                                }
                                else if (pair.Value >= tmp_index && pair.Value < i)
                                    storedIndices[pair.Key]++;
                            }
                            //Console.WriteLine("AFTER");
                            //foreach (var pair in storedIndices)
                                //Console.WriteLine($"{i} - Reference for {(pair.Key.Queue.Count == 0 ? "NULL" : pair.Key.Queue.Peek())} at index {pair.Value}");
                            // Bad I think because it isn't registering new links (i.e. 47|29)
                            // However, breaks worse when removed, so need to remove and debug that
                            //break;
                        }
                    }
                }

                foreach (Rule rule in _rules)
                {
                    rule.Reset();
                }

                /*foreach (int value in update)
                    Console.Write($"{value},");
                Console.WriteLine();*/

                sum += update[(update.Count - 1) / 2];
            }

            return new ValueTask<string>(sum.ToString());
        }
    }
}
