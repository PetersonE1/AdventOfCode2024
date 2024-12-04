using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    [RunTest]
    public class Day04 : TestableDay
    {
        private readonly string _input;

        public Day04()
        {
            _input = File.ReadAllText(InputFilePath);
        }

        public override ValueTask<string> Solve_1()
        {
            int count = 0;

            int uCount = 0;
            int bCount = 0;
            int fCount = 0;
            int dCount = 0;
            int fuCount = 0;
            int bdCount = 0;
            int fdCount = 0;
            int buCount = 0;

            int width = _input.IndexOf('\r');
            int height = _input.Length / (width + 1);
            string input = _input.Replace("\r\n", "");

            Console.WriteLine($"Width: {width}");
            Console.WriteLine($"Height: {height}");
            Console.WriteLine("Expected Result: 2554\n");

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (CheckForward(x, y, width, height, input))
                        fCount++; 
                    if (CheckBackward(x, y, width, height, input))
                        bCount++;
                    if (CheckDown(x, y, width, height, input))
                        dCount++;
                    if (CheckUp(x, y, width, height, input))
                        uCount++;
                    if (CheckForwardDown(x, y, width, height, input))
                        fdCount++;
                    if (CheckBackwardUp(x, y, width, height, input))
                        buCount++;
                    if (CheckForwardUp(x, y, width, height, input))
                        fuCount++;
                    if (CheckBackwardDown(x, y, width, height, input))
                        bdCount++;
                }
            }

            Console.WriteLine($"Forward: {fCount}");
            Console.WriteLine($"Backward: {bCount}");
            Console.WriteLine($"Down: {dCount}");
            Console.WriteLine($"Up: {uCount}");
            Console.WriteLine($"Forward Down: {fdCount}");
            Console.WriteLine($"Backward Up: {buCount}");
            Console.WriteLine($"Forward Up: {fuCount}");
            Console.WriteLine($"Backward Down: {bdCount}");

            count = uCount + bCount + fCount + dCount + fuCount + bdCount + fdCount + buCount;

            return new(count.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            throw new NotImplementedException();
        }

        // Cardinal Directions

        private bool CheckForward(int x, int y, int width, int height, string input)
        {
            int index = y * width + x;
            if ((index + 4) % width <= index % width)
                return false;
            string word = input.Substring(index, 4);
            //if (word == "XMAS") Console.WriteLine($"F [{x}, {y}] {word}");
            return word == "XMAS";
        }

        private bool CheckBackward(int x, int y, int width, int height, string input)
        {
            int index = y * width + x;
            if ((index - 4) % width >= index % width || index - 4 < 0)
                return false;
            string word = input.Substring(index - 4, 4);
            //if (word == "SAMX") Console.WriteLine($"B [{x}, {y}] {word}");
            return word == "SAMX";
        }

        private bool CheckDown(int x, int y, int width, int height, string input)
        {
            int index = y * width + x;
            if (index + 4 * width >= width * height)
                return false;
            string word = "";
            for (int i = 0; i < 4; i++)
            {
                word += input[index + i * width];
            }
            //if (word == "XMAS") Console.WriteLine($"D [{x}, {y}] {word}");
            return word == "XMAS";
        }

        private bool CheckUp(int x, int y, int width, int height, string input)
        {
            int index = y * width + x;
            if (index - 4 * width < 0)
                return false;
            string word = "";
            for (int i = 0; i < 4; i++)
            {
                word += input[index - i * width];
            }
            //if (word == "XMAS") Console.WriteLine($"U [{x}, {y}] {word}");
            return word == "XMAS";
        }

        // Diagonal Directions

        private bool CheckForwardDown(int x, int y, int width, int height, string input)
        {
            int index = y * width + x;
            if ((index + 4 * width + 4) >= width * height)
                return false;
            string word = "";
            for (int i = 0; i < 4; i++)
            {
                word += input[index + i * width + i];
            }
            //if (word == "XMAS") Console.WriteLine($"FD [{x}, {y}] {word}");
            return word == "XMAS";
        }

        private bool CheckBackwardUp(int x, int y, int width, int height, string input)
        {
            int index = y * width + x;
            if ((index - 4 * width - 4) < 0)
                return false;
            string word = "";
            for (int i = 0; i < 4; i++)
            {
                word += input[index - i * width - i];
            }
            //if (word == "XMAS") Console.WriteLine($"BU [{x}, {y}] {word}");
            return word == "XMAS";
        }

        private bool CheckForwardUp(int x, int y, int width, int height, string input)
        {
            int index = y * width + x;
            if ((index - 4 * width + 4) < 0)
                return false;
            string word = "";
            for (int i = 0; i < 4; i++)
            {
                word += input[index - i * width + i];
            }
            //if (word == "XMAS") Console.WriteLine($"FU [{x}, {y}] {word}");
            return word == "XMAS";
        }

        private bool CheckBackwardDown(int x, int y, int width, int height, string input)
        {
            int index = y * width + x;
            if ((index + 4 * width - 4) >= width * height)
                return false;
            string word = "";
            for (int i = 0; i < 4; i++)
            {
                word += input[index + i * width - i];
            }
            //if (word == "XMAS") Console.WriteLine($"BD [{x}, {y}] {word}");
            return word == "XMAS";
        }
    }
}
