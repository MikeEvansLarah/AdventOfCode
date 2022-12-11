namespace AdventOfCode.Solutions.Year2022.Day04;

class Solution : SolutionBase
{
    public Solution() : base(04, 2022, "Camp Cleanup") { }

    public record Assignment(int Start, int End);
    public class Pair
    {
        public Pair(Assignment assignment1, Assignment assignment2)
        {
            Assignment1 = assignment1;
            Assignment2 = assignment2;
        }

        public Assignment Assignment1 { get; }
        public Assignment Assignment2 { get; }

        public bool HasFullOverlap()
        {
            return
                (Assignment1.Start >= Assignment2.Start && Assignment1.End <= Assignment2.End) ||
                (Assignment2.Start >= Assignment1.Start && Assignment2.End <= Assignment1.End);
        }

        public bool HasAnyOverlap()
        {
            return
               (Assignment1.Start <= Assignment2.Start && Assignment1.End >= Assignment2.Start) ||
               (Assignment2.Start <= Assignment1.Start && Assignment2.End >= Assignment1.Start);
        }
    }

    protected override string SolvePartOne()
    {
        List<Pair> pairs = ParsePairs();

        return pairs.Count(p => p.HasFullOverlap()).ToString();
    }

    protected override string SolvePartTwo()
    {
        List<Pair> pairs = ParsePairs();

        return pairs.Count(p => p.HasAnyOverlap()).ToString();
    }

    private List<Pair> ParsePairs()
    {
        return this.Input!
                    .SplitByNewline()
                    .Select(x =>
                    {
                        var a1 = x.Split(',')[0];
                        var a2 = x.Split(',')[1];
                        var pair = new Pair(
                            new Assignment(int.Parse(a1.Split('-')[0]), int.Parse(a1.Split('-')[1])),
                            new Assignment(int.Parse(a2.Split('-')[0]), int.Parse(a2.Split('-')[1]))
                        );

                        return pair;
                    }).ToList();
    }
}
