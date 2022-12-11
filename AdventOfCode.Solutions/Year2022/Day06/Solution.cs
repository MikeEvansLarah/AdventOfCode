namespace AdventOfCode.Solutions.Year2022.Day06;

class Solution : SolutionBase
{
    public Solution() : base(06, 2022, "Tuning Trouble") { }

    private string FindMarkerIndex(int numDistinctCharacters)
    {
        for (int i = numDistinctCharacters; i < Input!.Length; i++)
        {
            string block = this.Input[(i - numDistinctCharacters)..i];

            if (block.Distinct().Count() == block.Length)
            {
                return i.ToString();
            }
        }

        throw new Exception("Marker not found.");
    }

    protected override string SolvePartOne()
    {
        const int numDistinctCharacters = 4;

        return FindMarkerIndex(numDistinctCharacters);
    }

    protected override string SolvePartTwo()
    {
        const int numDistinctCharacters = 14;

        return FindMarkerIndex(numDistinctCharacters);
    }
}
