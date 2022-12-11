namespace AdventOfCode.Solutions.Year2022.Day11;

class Solution : SolutionBase
{
    public Solution() : base(11, 2022, "Monkey in the Middle") { }

    public class Monkey
    {
        private int index;

        public Monkey(int index, Queue<Item> items, Func<long, long> operation, int testDivisor, int trueMonkeyIndex, int falseMonkeyIndex)
        {
            this.index = index;
            this.Items = items;
            this.Operation = operation;
            this.TestDivisor = testDivisor;
            this.TrueMonkeyIndex = trueMonkeyIndex;
            this.FalseMonkeyIndex = falseMonkeyIndex;
        }

        public Queue<Item> Items { get; set; }

        public Func<long, long> Operation { get; set; }

        public int TestDivisor { get; set; }

        public int TrueMonkeyIndex { get; set; }

        public int FalseMonkeyIndex { get; set; }

        public long ItemsInspected = 0;
    }

    public class Item
    {
        public Item(long worryLevel)
        {
            WorryLevel = worryLevel;
        }

        public long WorryLevel { get; set; }
    }

    public IList<Monkey> Parse()
    {
        return this.Input!.SplitByParagraph()
            .Select(x =>
            {
                string[] lines = x.SplitByNewline(true).ToArray();
                int index = int.Parse(lines[0][7..^1]);
                var items = lines[1][16..].Split(",").Select(x => new Item(int.Parse(x))).ToList();
                char op = lines[2][21];
                string operandStr = lines[2][23..];

                Func<long, long> operation;
                if (operandStr == "old")
                {
                    operation = old => old * old;
                }
                else
                {
                    int operand = int.Parse(operandStr);
                    switch (op)
                    {
                        case '+':
                            operation = old => old + operand;
                            break;
                        case '*':
                            operation = old => old * operand;
                            break;
                        default:
                            throw new Exception("Unknown operator");
                    }
                }

                int testDivisor = int.Parse(lines[3][19..]);
                int trueMonkeyIndex = int.Parse(lines[4][25..]);
                int falseMonkeyIndex = int.Parse(lines[5][26..]);

                return new Monkey(
                    index,
                    new Queue<Item>(items),
                    operation,
                    testDivisor,
                    trueMonkeyIndex,
                    falseMonkeyIndex
                );

            }).ToList();
    }

    public static IList<Monkey> ProcessRound(IList<Monkey> monkeys, bool divideByThree = true)
    {
        var lcm = monkeys
            .Select(m => m.TestDivisor)
            .Aggregate((a, b) => CalculationUtils.FindLCM(a, b));

        foreach (var monkey in monkeys)
        {
            while (monkey.Items.TryDequeue(out var item))
            {
                item.WorryLevel = monkey.Operation(item.WorryLevel);

                if (divideByThree) item.WorryLevel /= 3;
                else item.WorryLevel %= lcm;

                monkey.ItemsInspected++;

                int nextMonkeyIndex = 
                    item.WorryLevel % monkey.TestDivisor == 0 
                    ? monkey.TrueMonkeyIndex 
                    : monkey.FalseMonkeyIndex;

                monkeys[nextMonkeyIndex].Items.Enqueue(item);
            }
        }

        return monkeys;
    }

    protected override string SolvePartOne()
    {
        var monkeys = this.Parse();
        for (int i = 0; i < 20; i++)
        {
            monkeys = ProcessRound(monkeys);
        }

        return monkeys
            .OrderByDescending(m => m.ItemsInspected)
            .Take(2)
            .Select(m => m.ItemsInspected)
            .Aggregate((a, b) => a * b)
            .ToString();
    }

    protected override string SolvePartTwo()
    {
        var monkeys = this.Parse();
        for (int i = 0; i < 10000; i++)
        {
            monkeys = ProcessRound(monkeys, false);
        }

        return monkeys
            .OrderByDescending(m => m.ItemsInspected)
            .Take(2)
            .Select(m => m.ItemsInspected)
            .Aggregate((a, b) => a * b)
            .ToString();
    }
}
