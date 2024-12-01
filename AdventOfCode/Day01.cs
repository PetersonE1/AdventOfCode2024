namespace AdventOfCode;

// This is how to run using the test input
// [RunTest]
public class Day01 : TestableDay
{
    private readonly string _input;
    private (int[] left, int[] right) _numbers;

    public Day01()
    {
        _input = File.ReadAllText(InputFilePath);

        string[] lines = _input.Split("\r\n");
        _numbers = new (new int[lines.Length], new int[lines.Length]);
        for (int i = 0; i < lines.Length; i++)
        {
            string[] digits = lines[i].Split("   ");
            _numbers.left[i] = int.Parse(digits[0]);
            _numbers.right[i] = int.Parse(digits[1]);
        }
    }

    public override ValueTask<string> Solve_1()
    {
        var orderedLeft = _numbers.left.Order();
        var orderedRight = _numbers.right.Order();

        int sum = 0;
        for (int i = 0; i < orderedLeft.Count(); i++)
        {
            int diff = orderedLeft.ElementAt(i) - orderedRight.ElementAt(i);
            sum += diff < 0 ? -diff : diff;
        }
        return new(sum.ToString());
    }

    public override ValueTask<string> Solve_2() => new(_numbers.left.Sum(i => i * _numbers.right.Count(n => n == i)).ToString());
}
