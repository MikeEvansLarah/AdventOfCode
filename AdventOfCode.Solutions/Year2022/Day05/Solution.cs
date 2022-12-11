using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2022.Day05;

partial class Solution : SolutionBase
{
    public Solution() : base(05, 2022, "Supply Stacks") { }

    public class Ship
    {
        public Ship(Stack<char>[] crateStacks, IList<Instruction> instructions)
        {
            CrateStacks = crateStacks;
            Instructions = instructions;
        }

        public Stack<char>[] CrateStacks { get; }

        public IList<Instruction> Instructions { get; }

        public void RearrangeCrates()
        {
            foreach (var instruction in Instructions)
            {
                for (int i = 0; i < instruction.NumberOfCrates; i++)
                {
                    var crate = CrateStacks[instruction.FromStack - 1].Pop();
                    CrateStacks[instruction.ToStack - 1].Push(crate);
                }
            }
        }

        public void RearrangeCrates9001()
        {
            foreach (var instruction in Instructions)
            {
                List<char> crates = new();
                for (int i = 0; i < instruction.NumberOfCrates; i++)
                {
                    var crate = CrateStacks[instruction.FromStack - 1].Pop();
                    crates.Add(crate);
                }

                crates.Reverse();

                foreach (var crate in crates)
                {
                    CrateStacks[instruction.ToStack - 1].Push(crate);
                }
            }
        }

        public string GetTopCrates()
        {
            return string.Join("", CrateStacks.Select(x => { x.TryPeek(out var crate); return crate; }));
        }
    }

    public record Instruction(int NumberOfCrates, int FromStack, int ToStack);

    public Ship Parse()
    {
        string[] paragraphs = Input!.SplitByParagraph().ToArray();
        string drawingPara = paragraphs[0];
        string instructionsPara = paragraphs[1];

        List<string> crateRows = drawingPara
            .SplitByNewline()
            .Reverse()
            .Skip(1)
            .Select(x =>
            {
                var sb = new StringBuilder();
                for (int i = 1; i < x.Length; i+=4)
                {
                    sb.Append(x[i]);
                }
                return sb.ToString();
            })
            .ToList();

        var crateStacks = Enumerable.Range(0, crateRows[0].Length).Select(_ => new Stack<char>()).ToArray();

        foreach (var crateRow in crateRows)
        {
            for (int i = 0; i < crateRow.Length; i++)
            {
                var crate = crateRow[i];
                if (crate == ' ') continue;
                crateStacks[i].Push(crate);
            }
        }

        var instructions = instructionsPara.SplitByNewline().Select(x =>
        {
            Regex rg = InstructionRegex();
            var matches = rg.Matches(x);
            return new Instruction(
                int.Parse(matches[0].Groups[1].Value), 
                int.Parse(matches[0].Groups[2].Value), 
                int.Parse(matches[0].Groups[3].Value)
            );
        }).ToList();

        return new Ship(crateStacks, instructions);
    }

    [GeneratedRegex(@"move (\d*) from (\d*) to (\d*)")]
    private static partial Regex InstructionRegex();

    protected override string SolvePartOne()
    {
        var ship = Parse();
        ship.RearrangeCrates();
   
        return ship.GetTopCrates();
    }

    protected override string SolvePartTwo()
    {
        var ship = Parse();
        ship.RearrangeCrates9001();

        return ship.GetTopCrates();
    }
}
