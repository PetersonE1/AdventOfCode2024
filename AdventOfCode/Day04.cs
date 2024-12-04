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
                    if (input[y * width + x] != 'X')
                        continue;
                    if (CheckHorizontal(x, y, width, height, input))
                        count++; 
                    if (CheckVertical(x, y, width, height, input))
                        count++;
                    if (CheckDiagonal_1(x, y, width, height, input))
                        count++;
                    if (CheckDiagonal_2(x, y, width, height, input))
                        count++;
                }
            }

            return new(count.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            throw new NotImplementedException();
        }

        // Cardinal Directions

        private bool CheckHorizontal(int x, int y, int width, int height, string input)
        {
            int index = y * width + x;
            if ((index + 4) % width <= index % width)
                return false;
            string word = input.Substring(index, 4);
            if (word == "XMAS" || word == "SAMX") Console.WriteLine($"Horizontal: [{x}, {y}]");
            return word == "XMAS" || word == "SAMX";
        }

        private bool CheckVertical(int x, int y, int width, int height, string input)
        {
            int index = y * width + x;
            if (index + (width * 4) > width * height)
                return false;
            string word = "";
            for (int i = 0; i < 4; i++)
            {
                word += input[index + (width * i)];
            }
            if (word == "XMAS" || word == "SAMX") Console.WriteLine($"Vertical: [{x}, {y}]");
            return word == "XMAS" || word == "SAMX";
        }

        // Diagonal Directions

        private bool CheckDiagonal_1(int x, int y, int width, int height, string input)
        {
            int index = y * width + x;
            if ((index + 4) + (width * 4) > width * height)
                return false;
            string word = "";
            for (int i = 0; i < 4; i++)
            {
                word += input[(index + i) + (width * i)];
            }
            if (word == "XMAS" || word == "SAMX") Console.WriteLine($"Diagonal ┘: [{x}, {y}]");
            return word == "XMAS" || word == "SAMX";
        }

        private bool CheckDiagonal_2(int x, int y, int width, int height, string input)
        {
            int index = y * width + x;
            if ((index + 4) - (width * 4) < 0)
                return false;
            string word = "";
            for (int i = 0; i < 4; i++)
            {
                word += input[(index + i) - (width * i)];
            }
            if (word == "XMAS" || word == "SAMX") Console.WriteLine($"Diagonal ┐: [{x}, {y}]");
            return word == "XMAS" || word == "SAMX";
        }
    }
}
