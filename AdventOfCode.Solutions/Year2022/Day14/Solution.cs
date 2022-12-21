using Spectre.Console;

namespace AdventOfCode.Solutions.Year2022.Day14;

class Solution : SolutionBase
{
    public Solution() : base(14, 2022, "Regolith Reservoir") { }

    public enum Material
    {
        Air,
        Rock,
        Sand
    }

    public class Cave
    {
        private readonly Material[,] map;
        private readonly int minX;

        public Cave(Material[,] map, int minX)
        {
            this.map = map;
            this.minX = minX;
        }

        public void Draw()
        {
            var sb = new StringBuilder();
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = minX; x < map.GetLength(0); x++)
                {
                    switch (map[x, y])
                    {
                        case Material.Air:
                            sb.Append(Emoji.Known.BlueSquare);
                            break;
                        case Material.Rock:
                            sb.Append(Emoji.Known.Rock);
                            break;
                        case Material.Sand:
                            sb.Append(Emoji.Known.BrownSquare);
                            break;
                        default:
                            break;
                    }
                }

                sb.Append(Environment.NewLine);
            }

            AnsiConsole.Markup(sb.ToString());
        }

        public void DropUnitOfSand(out bool isFull, bool draw = false)
        {
            (int sandX, int sandY) = (500, 0);

            bool canMove = true;

            do
            {
                if (map[sandX, sandY] == Material.Sand)
                {
                    isFull = true;
                    return;
                }
                else if (sandX + 1 > map.GetLength(0) || sandX < minX || sandY + 1 >= map.GetLength(1))
                {
                    isFull = true;
                    return;
                }
                else
                {
                    isFull = false;
                }

                if (map[sandX, sandY + 1] == Material.Air)
                {
                    sandY++;
                }
                else
                {
                    if (map[sandX - 1, sandY + 1] == Material.Air)
                    {
                        sandX--;
                        sandY++;
                    }
                    else if (map[sandX + 1, sandY + 1] == Material.Air)
                    {
                        sandX++;
                        sandY++;
                    }
                    else
                    {
                        canMove = false;
                    }
                }
            }
            while (canMove);

            map[sandX, sandY] = Material.Sand;

            if (draw)
            {
                AnsiConsole.Cursor.SetPosition(sandX - minX/2, sandY + 1);
                AnsiConsole.Markup(Emoji.Known.BrownSquare);
                Thread.Sleep(TimeSpan.FromTicks(100));
            }

            return;
        }

        public static Cave Parse(string input, bool addFloor = false)
        {
            var lines = ParseLines(input).ToList();
            var coordinatesFlattened = lines.SelectMany(l => l).ToList();

            int minX = coordinatesFlattened.Min(c => c.x);
            int maxX = coordinatesFlattened.Max(c => c.x);
            int maxY = coordinatesFlattened.Max(c => c.y);

            if (addFloor)
            {
                maxY += 2;
                maxX += 150;
                minX = 330;
            }

            var map = new Material[maxX + 1, maxY + 1];

            foreach (var line in lines)
            {
                for (int i = 1; i < line.Count; i++)
                {
                    (int lastX, int lastY) = line[i - 1];
                    (int currentX, int currentY) = line[i];

                    if (lastX == currentX)
                    {
                        if (lastY < currentY)
                        {
                            for (int y = lastY; y <= currentY; y++)
                            {
                                map[currentX, y] = Material.Rock;
                            }
                        }
                        else
                        {
                            for (int y = lastY; y >= currentY; y--)
                            {
                                map[currentX, y] = Material.Rock;
                            }
                        }
                        
                    }

                    if (lastY == currentY)
                    {
                        if (lastX < currentX)
                        {
                            for (int x = lastX; x <= currentX; x++)
                            {
                                map[x, currentY] = Material.Rock;
                            }
                        }
                        else
                        {
                            for (int x = lastX; x >= currentX; x--)
                            {
                                map[x, currentY] = Material.Rock;
                            }
                        }                       
                    }
                }
            }

            if (addFloor)
            {
                for (int x = 0; x <= maxX; x++)
                {
                    map[x, maxY] = Material.Rock;
                }
            }

            return new Cave(map, minX);
        }

        private static IEnumerable<IList<(int x, int y)>> ParseLines(string input)
        {
            foreach (var line in input.SplitByNewline())
            {
                yield return ParseCoordinates(line).ToList();
            }
        }

        private static IEnumerable<(int x, int y)> ParseCoordinates(string line)
        {
            foreach (var coordinates in line.Split(" -> "))
            {
                var (x, y, _) = coordinates.Split(",").Select(int.Parse).ToList();
                yield return (x, y);
            }
        }
    }

    protected override string SolvePartOne()
    {
        var cave = Cave.Parse(this.Input!);

        bool isFull = false;
        int units = 0;

        while (!isFull)
        {
            cave.DropUnitOfSand(out isFull);
            if (!isFull) units++;
        }

        Console.Clear();
        cave.Draw();
        Console.WriteLine();

        return units.ToString();
    }

    protected override string SolvePartTwo()
    {
        var cave = Cave.Parse(this.Input!, true);

        Console.Clear();
        Console.CursorVisible = false;
        cave.Draw();

        bool isFull = false;
        int units = 0;

        while (!isFull)
        {
            cave.DropUnitOfSand(out isFull, true);
            if (!isFull) units++;
        }

        return units.ToString();
    }
}
