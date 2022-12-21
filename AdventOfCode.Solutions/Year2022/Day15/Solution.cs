using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2022.Day15;

class Solution : SolutionBase
{
    public Solution() : base(15, 2022, "Beacon Exclusion Zone") { }

    public class Sensor
    {
        static Regex regex = new(@"Sensor at x\=(.*), y\=(.*)\: closest beacon is at x\=(.*), y\=(.*)");

        public Sensor((int x, int y) coordinates, (int x, int y) closestBeaconCoordinates)
        {
            Coordinates = coordinates;
            ClosestBeaconCoordinates = closestBeaconCoordinates;
        }

        public (int x, int y) Coordinates { get; }
        public (int x, int y) ClosestBeaconCoordinates { get; }

        public static Sensor Parse(string input)
        {
            var groups = regex.Match(input).Groups;
            
            var sensor = new Sensor(
                (int.Parse(groups[1].Value), int.Parse(groups[2].Value)), 
                (int.Parse(groups[3].Value), int.Parse(groups[4].Value))
            );

            return sensor;
        }
    }

    protected override string SolvePartOne()
    {
        var sensors = this.Input!.SplitByNewline().Select(Sensor.Parse).ToList();

        List<int> interestingRow = new();
        int interestingRowY = 2000000;
        //int interestingRowY = 10;

        foreach (var sensor in sensors)
        {
            var manhattanDistanceToBeacon = CalculationUtils.ManhattanDistance(sensor.Coordinates, sensor.ClosestBeaconCoordinates);

            var beaconXDistanceAtInterestingRow = manhattanDistanceToBeacon - Math.Abs(interestingRowY - sensor.Coordinates.y);

            if (beaconXDistanceAtInterestingRow > 0)
            {
                interestingRow.AddRange(Enumerable.Range(sensor.Coordinates.x - beaconXDistanceAtInterestingRow, beaconXDistanceAtInterestingRow * 2));
            }      
        }

        var result = interestingRow.Distinct().Count();

        return result.ToString();
    }

    protected override string SolvePartTwo()
    {
        var sensors = this.Input!.SplitByNewline().Select(Sensor.Parse).ToList();

        Dictionary<int, List<(int minX, int maxX)>> rows = new();

        foreach (var sensor in sensors)
        {
            var manhattanDistanceToBeacon = CalculationUtils.ManhattanDistance(sensor.Coordinates, sensor.ClosestBeaconCoordinates);

            for (int y = 0; y < 4000000; y++)
            {
                var beaconXDistanceAtRow = manhattanDistanceToBeacon - Math.Abs(y - sensor.Coordinates.y);

                if (beaconXDistanceAtRow > 0)
                {
                    int minX = sensor.Coordinates.x - beaconXDistanceAtRow;
                    int maxX = sensor.Coordinates.x + beaconXDistanceAtRow;

                    if (!rows.ContainsKey(y))
                    {
                        rows[y] = new List<(int, int)>();
                    }

                    rows[y].Add((minX, maxX));
                }
            }
        }

        (int x, int y)? result = null;
        foreach (var row in rows)
        {
            var ranges = row.Value.OrderBy(r => r.minX).ToList();
            int maxX = 0;

            for (int i = 0; i < ranges.Count - 1; i++)
            {
                if (ranges[i].maxX > maxX) 
                {
                    maxX = ranges[i].maxX;
                }

                if (ranges[i + 1].minX > maxX + 1)
                {
                    result = (ranges[i].maxX + 1, row.Key);
                    break;
                }
            }
        }

        if (result == null)
        {
            throw new Exception("Something's gone wrong");
        }

        return (result.Value.x * (long)4000000 + result.Value.y).ToString();
    }
}
