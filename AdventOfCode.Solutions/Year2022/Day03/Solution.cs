using static AdventOfCode.Solutions.Year2022.Day03.Solution;

namespace AdventOfCode.Solutions.Year2022.Day03;

class Solution : SolutionBase
{
    public Solution() : base(03, 2022, "Rucksack Reorganization") { }

    public class Rucksack
    {
        public Rucksack(string contents)
        {
            this.Contents = contents;
            this.FirstCompartment = contents[..(contents.Length / 2)];
            this.SecondCompartment = contents[(contents.Length / 2)..];
        }

        public string Contents { get; }
        public string FirstCompartment { get; }
        public string SecondCompartment { get; }

        public char FindSharedItem()
        {
            foreach (char c1 in FirstCompartment)
            {
                foreach (char c2 in SecondCompartment)
                {
                    if (c1 == c2) return c1;
                }
            }

            throw new Exception("Shared item not found");
        }
    }

    public static int CharToPriority(char c)
    {
        return "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(c) + 1;
    }

    public static char FindCommonItem(IList<Rucksack> rucksacks)
    {
        foreach (char c1 in rucksacks[0].Contents)
        {
            foreach (char c2 in rucksacks[1].Contents)
            {
                if (c1 == c2)
                {
                    foreach (char c3 in rucksacks[2].Contents)
                    {
                        if (c1 == c3)
                        {
                            return c1;
                        }
                    }
                }
            }
        }

        throw new Exception("Common item not found");
    }

    protected override string SolvePartOne()
    {
        var rucksacks = this.Input.SplitByNewline().Select(
            x => new Rucksack(x)
        ).ToList();

        var sum = rucksacks.Select(r => r.FindSharedItem()).Select(CharToPriority).Sum();

        return sum.ToString();
    }

    protected override string SolvePartTwo()
    {
        var rucksacks = this.Input.SplitByNewline().Select(
            x => new Rucksack(x)
        ).ToList();

        var sum = rucksacks.Chunk(3).Select(FindCommonItem).Select(CharToPriority).Sum();

        return sum.ToString();
    }
}
