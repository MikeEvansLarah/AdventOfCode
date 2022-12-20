using System.Text.Json;
using static AdventOfCode.Solutions.Year2022.Day13.Solution;

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

        public static bool? OrderedCorrectly(Packet left, Packet right)
        {
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

                    var ordered = OrderedCorrectly(left.List[i], right.List[i]);

                    if (ordered.HasValue)
                    {
                        return ordered;
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            else if (left.Integer.HasValue)
            {
                return OrderedCorrectly(new Packet { List = new List<Packet> { left } }, right);
            }

            else if (right.Integer.HasValue)
            {
                return OrderedCorrectly(left, new Packet { List = new List<Packet> { right } });
            }

            return null;
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

                    return (new Packet { List = packets }, i + 1);
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
            .Select(
                x => x.SplitByNewline().Select(Packet.Parse).ToList())
            .ToList();

        int result = 0;
        for (int i = 0; i < pairs.Count; i++)
        {
            var pair = pairs[i];
            var orderedCorrectly = Packet.OrderedCorrectly(pair[0], pair[1]);

            if (orderedCorrectly.HasValue && orderedCorrectly.Value)
            {
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
