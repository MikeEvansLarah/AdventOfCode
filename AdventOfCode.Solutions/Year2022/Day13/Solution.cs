namespace AdventOfCode.Solutions.Year2022.Day13;

class Solution : SolutionBase
{
    public Solution() : base(13, 2022, "Distress Signal") { }

    public class Pair
    {
        private Pair(Packet p1, Packet p2) 
        {
            Packet1 = p1;
            Packet2 = p2;
        }

        public Packet Packet1 { get; set; }

        public Packet Packet2 { get; set; }

        public static Pair Parse(string input)
        {
            var lines = input.SplitByNewline().ToArray();

            return new Pair(
                Packet.Parse(lines[0]),
                Packet.Parse(lines[1])
            );
        }

        public bool OrderedCorrectly()
        {
            var left = Packet1;
            var right = Packet2;

            return Compare(left, right) < 0;
        }

        public static int Compare(Packet left, Packet right)
        {
            if (left.Integer.HasValue && right.Integer.HasValue)
            {
                if (left.Integer.Value < right.Integer.Value)
                {
                    return -1;
                }
                else if (left.Integer.Value > right.Integer.Value)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

            else if (!left.Integer.HasValue && !right.Integer.HasValue)
            {
                for (int i = 0; i < Math.Max(left.List.Count, right.List.Count); i++)
                {
                    if (left.List.Count < i + 1 && right.List.Count >= i + 1) return -1;
                    if (right.List.Count < i + 1 && left.List.Count >= i + 1) return 1;

                    var ordered = Compare(left.List[i], right.List[i]);

                    if (ordered != 0)
                    {
                        return ordered;
                    }
                }
            }

            else if (left.Integer.HasValue)
            {
                return Compare(new Packet { List = new List<Packet> { left } }, right);
            }

            else if (right.Integer.HasValue)
            {
                return Compare(left, new Packet { List = new List<Packet> { right } });
            }

            return 0;
        }
    }

    public class Packet
    {
        public List<Packet> List { get; set; } = new();

        public int? Integer { get; set; }

        public bool IsDivider { get; set; }

        public static Packet Parse(string input)
        {
            var (packet, _) = ParseInternal(input);

            if (input == "[[2]]" || input == "[[6]]")
            {
                packet.IsDivider = true;
            }

            return packet;
        }

        private static (Packet packet, int increment) ParseInternal(string input)
        {
            var valueSb = new StringBuilder();
            List<Packet> packets = new();

            for (int i = 1; i < input.Length; i++)
            {
                char c = input[i];

                if (c == '[')
                {
                    var (packet, increment) = ParseInternal(input[i..]);
                    i += increment;
                    packets.Add(packet);
                }
                else if (c == ']')
                {
                    if (valueSb.Length > 0)
                    {
                        int value = int.Parse(valueSb.ToString());
                        packets.Add(new Packet { Integer = value });
                    }

                    return (new Packet { List = packets }, i);
                }
                else if (c == ',')
                {
                    if (valueSb.Length > 0)
                    {
                        int value = int.Parse(valueSb.ToString());
                        packets.Add(new Packet { Integer = value });
                        valueSb = new StringBuilder();
                    }
                }
                else
                {
                    valueSb.Append(c);
                }
            }

            return (new Packet { List = packets }, input.Length);
        }
    }


    protected override string SolvePartOne()
    {
        var pairs = this.Input!.SplitByParagraph()
            .Select(Pair.Parse)
            .ToList();

        int result = 0;
        for (int i = 0; i < pairs.Count; i++)
        {
            var pair = pairs[i];
            var orderedCorrectly = pair.OrderedCorrectly();

            if (orderedCorrectly)
            {
                result += (i + 1);
            }
        }

        return result.ToString();
    }

    protected override string SolvePartTwo()
    {
        var packets = this.Input!.SplitByNewline(true).Select(Packet.Parse).ToList();
        packets.Add(Packet.Parse("[[2]]"));
        packets.Add(Packet.Parse("[[6]]"));

        var sorted = packets.BubbleSort(Pair.Compare);

        var result = sorted.Select((value, i) => new { value, index = i + 1 })
            .Where(pair => pair.value.IsDivider)
            .Aggregate(1, (acc, pair) => pair.index * acc);

        return result.ToString();
    }
}
