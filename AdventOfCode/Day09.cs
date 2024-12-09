using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    //[RunTest]
    public class Day09 : TestableDay
    {
        private readonly string _input;
        private readonly int[] _diskmap;

        public Day09()
        {
            _input = File.ReadAllText(InputFilePath);
            bool mappingEmpty = false;
            List<int> diskmap = new();
            int id = 0;
            foreach (char c in _input)
            {
                if (mappingEmpty)
                {
                    for (int i = 0; i < c - 48 /* Shift char representing int to actual int */; i++)
                        diskmap.Add(-1);
                }
                else
                {
                    for (int i = 0; i < c - 48; i++)
                        diskmap.Add(id);
                    id++;
                }
                mappingEmpty = !mappingEmpty;
            }
            _diskmap = diskmap.ToArray();
        }

        // Slow, needs to work inwards constantly; currently reloops through the entire array each time
        public override ValueTask<string> Solve_1()
        {
            while (!IsDense(_diskmap))
            {
                int emptyIndex = Array.IndexOf(_diskmap, -1);
                int idIndex = Array.FindLastIndex(_diskmap, n => n != -1);

                _diskmap[emptyIndex] = _diskmap[idIndex];
                _diskmap[idIndex] = -1;
            }
            return new ValueTask<string>(Checksum(_diskmap).ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            throw new NotImplementedException();
        }

        private ulong Checksum(int[] diskmap)
        {
            ulong checksum = 0;
            for (uint i = 0; i < diskmap.Length; i++)
            {
                if (diskmap[i] == -1)
                    break;
                checksum += (ulong)diskmap[i] * i;
            }
            return checksum;
        }

        private bool IsDense(int[] diskmap)
        {
            bool checkingEmpty = false;
            foreach (int i in diskmap)
            {
                if (checkingEmpty && i != -1)
                    return false;
                if (!checkingEmpty && i == -1)
                    checkingEmpty = true;
            }
            return true;
        }

        private void PrintDiskmap()
        {
            for (int i = 0; i < _diskmap.Length; i++)
            {
                Console.Write(_diskmap[i] == -1 ? '.' : _diskmap[i].ToString());
            }
            Console.WriteLine();
        }
    }
}
