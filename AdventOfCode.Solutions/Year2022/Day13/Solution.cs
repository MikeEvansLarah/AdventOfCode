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

        public bool? OrderedCorrectly()
        {
            var left = Packet1;
            var right = Packet2;

            if (left.Integer.HasValue && right.Integer.HasValue)
            {
                if (left.Integer.Value < right.Integer.Value)
                {
                    return true;
                }
                else if (left.Integer.Value > right.Integer.Value)
                {
                    return false;
                }
                else
                {
                    return null;
                }
            }

            else if (!left.Integer.HasValue && !right.Integer.HasValue)
            {
                for (int i = 0; i < Math.Max(left.List.Count, right.List.Count); i++)
                {
                    if (left.List.Count < i + 1 && right.List.Count >= i + 1) return true;
                    if (right.List.Count < i + 1 && left.List.Count >= i + 1) return false;

                    var pair = new Pair(left.List[i], right.List[i]);
                    var ordered = pair.OrderedCorrectly();

                    if (ordered.HasValue)
                    {
                        return ordered;
                    }
                }
            }

            else if (left.Integer.HasValue)
            {
                var pair = new Pair(new Packet { List = new List<Packet> { left } }, right);
                return pair.OrderedCorrectly();
            }

            else if (right.Integer.HasValue)
            {
                var pair = new Pair(left, new Packet { List = new List<Packet> { right } });
                return pair.OrderedCorrectly();
            }

            return null;
        }
    }

    public class Packet
    {
        public List<Packet> List { get; set; } = new();

        public int? Integer { get; set; }

        public static Packet Parse(string input)
        {
            var (packet, _) = ParseInternal(input);
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

            if (orderedCorrectly.HasValue && orderedCorrectly.Value)
            {
                Console.WriteLine(i + 1);
                result += (i + 1);
            }
        }

        return result.ToString();
    }

    protected override string SolvePartTwo()
    {
        return "";
    }
}
