using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    [RunTest]
    public class Day09 : TestableDay
    {
        private readonly string _input;
        private readonly int[] _diskmap;

        private readonly SortableList<AmphiFile> _amphiFiles;

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

            List<AmphiFile> amphiFiles = new();
            id = 0;
            for (int i = 0; i < _input.Length; i += 2)
            {
                int length = _input[i] - 48;
                int gap = (i + 1) >= _input.Length ? 0 : _input[i + 1] - 48;
                amphiFiles.Add(new AmphiFile(id++, length, gap));
            }
            _amphiFiles = new SortableList<AmphiFile>(amphiFiles);
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

        private void PrintAmphiFiles()
        {
            foreach (AmphiFile file in _amphiFiles.Values)
            {
                for (int i = 0; i < file.Length; i++)
                    Console.Write(file.ID);
                for (int i = 0; i < file.TrailingVoid; i++)
                    Console.Write('.');
            }
            Console.WriteLine();
        }

        // Going to bed; work on this later.
        // Goal: Be able to iterate through items without breaking the iter, while being able to move them in a sorted list.
        // Need to be able to track order of items to iterate through (in order) to check if the gap on the right is big enough to move.
        // Also need to not actually move items being iterated so I can still do a simple iteration from highest ID to lowest.
        // When moved left: Add length + gap to the item to the left, move to the left, set the gap of the item to the left to zero, set gap to left item's previous gap - length.
        private class SortableList<T>
        {
            public List<T> Items { get; set; }
            public List<T> SortedItems { get; set; }

            public SortableList(List<T> values)
            {
                Items = new List<T>(values);
                SortedItems = new List<T>(values);
            }

            public void Move(int from, int to)
            {
                T value = Values[from];
                Values.RemoveAt(from);
                Values.Insert(to, value);
            }
        }

        private class AmphiFile
        {
            public int ID { get; private set; }
            public int Length { get; private set; }
            public int TrailingVoid { get; set; }

            public AmphiFile(int id, int length, int gap)
            {
                ID = id;
                Length = length;
                TrailingVoid = gap;
            }
        }
    }
}
