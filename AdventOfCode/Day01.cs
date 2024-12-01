namespace AdventOfCode;

// This is how to run using the test input
// [RunTest]
public class Day01 : TestableDay
{
    private readonly string _input;
    private (List<int> Left, List<int> Right) _numbers = new (new (), new ());

    public Day01()
    {
        _input = File.ReadAllText(InputFilePath);

        foreach (string line in _input.Split("\r\n"))
        {
            int[] digits = line.Split("   ").Select(n => int.Parse(n)).ToArray();
            _numbers.Left.Add(digits[0]);
            _numbers.Right.Add(digits[1]);
        }

        _numbers.Left.Sort();
        _numbers.Right.Sort();
    }

    public override ValueTask<string> Solve_1()
    {
        int sum = 0;
        for (int i = 0; i < _numbers.Left.Count(); i++)
        {
            int diff = _numbers.Left[i] - _numbers.Right[i];
            sum += Math.Abs(diff);
        }
        return new(sum.ToString());
    }

    public override ValueTask<string> Solve_2() => new(_numbers.Left.Sum(i => i * _numbers.Right.Count(n => n == i)).ToString());
}
