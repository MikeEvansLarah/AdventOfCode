namespace AdventOfCode.Solutions.Year2022.Day09;

class Solution : SolutionBase
{
    public Solution() : base(09, 2022, "Rope Bridge") { }

    static (int x, int y) MoveHead((int x, int y) headPosition, char direction)
    {
        switch (direction)
        {
            case 'L':
                headPosition.x--;
                break;
            case 'R':
                headPosition.x++;
                break;
            case 'D':
                headPosition.y--;
                break;
            case 'U':
                headPosition.y++;
                break;
            default:
                break;
        }

        return headPosition;
    }

    static (int x, int y) MoveTail((int x, int y) followPosition, (int x, int y) tailPosition)
    {
        var maxDistance = 1;

        if (followPosition.x < tailPosition.x - maxDistance)
        {
            tailPosition.x--;
            if (followPosition.y > tailPosition.y) { tailPosition.y++; }
            if (followPosition.y < tailPosition.y) { tailPosition.y--; }
        }
        else if (followPosition.x > tailPosition.x + maxDistance)
        {
            tailPosition.x++;
            if (followPosition.y > tailPosition.y) { tailPosition.y++; }
            if (followPosition.y < tailPosition.y) { tailPosition.y--; }
        }
        else if (followPosition.y < tailPosition.y - maxDistance)
        {
            tailPosition.y--;
            if (followPosition.x > tailPosition.x) { tailPosition.x++; }
            if (followPosition.x < tailPosition.x) { tailPosition.x--; }
        }
        else if (followPosition.y > tailPosition.y + maxDistance)
        {
            tailPosition.y++;
            if (followPosition.x > tailPosition.x) { tailPosition.x++; }
            if (followPosition.x < tailPosition.x) { tailPosition.x--; }
        }

        return tailPosition;
    }

    protected override string SolvePartOne()
    {
        var visitedPositions = new HashSet<(int, int)>();
        var headPosition = (x:0, y:0);
        var tailPosition = (x:0, y:0);
        visitedPositions.Add(tailPosition);

        string[] lines = this.Input.SplitByNewline();
        for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
        {
            string? line = lines[lineIndex];
            char direction = line[0];
            int steps = int.Parse(line[2..]);

            for (int i = 0; i < steps; i++)
            {
                headPosition = MoveHead(headPosition, direction);

                tailPosition = MoveTail(headPosition, tailPosition);

                visitedPositions.Add(tailPosition);
            }
        }

        return visitedPositions.Count.ToString();     
    }

    protected override string SolvePartTwo()
    {
        var visitedPositions = new HashSet<(int, int)>();
        var headPosition = (x: 0, y: 0);
        var tailPositions = new (int x, int y)[9];
        
        visitedPositions.Add(tailPositions[^1]);

        string[] lines = this.Input.SplitByNewline();
        for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
        {
            string? line = lines[lineIndex];
            char direction = line[0];
            int steps = int.Parse(line[2..]);

            for (int i = 0; i < steps; i++)
            {
                headPosition = MoveHead(headPosition, direction);

                for (int j = 0; j < tailPositions.Length; j++)
                {
                    tailPositions[j] = MoveTail(
                        j == 0 ? headPosition: tailPositions[j - 1], 
                        tailPositions[j]
                    );
                }

                visitedPositions.Add(tailPositions[^1]);
            }
        }

        return visitedPositions.Count.ToString();
    }
}
