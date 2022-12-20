namespace AdventOfCode.Solutions.Year2022.Day12;

class Solution : SolutionBase
{
    public Solution() : base(12, 2022, "Hill Climbing Algorithm") { }

    public class Map
    {
        public Map(int[,] grid, (int, int) start, (int, int) end, IList<(int, int)> otherStarts)
        {
            Grid = grid;
            Start = start;
            End = end;
            OtherStarts = otherStarts;
        }

        public int[,] Grid { get; }

        public (int, int) Start { get; }

        public (int, int) End { get; }

        public IList<(int, int)> OtherStarts { get; }

        public Graph<(int, int)> ToGraph()
        {
            var vertices = new List<(int, int)>();
            var edges = new List<Tuple<(int, int), (int, int)>>();
            
            int gridHeight = Grid.GetLength(0);
            int gridWidth = Grid.GetLength(1);

            for (int i = 0; i < gridHeight; i++)
            {
                for (int j = 0; j < gridWidth; j++)
                {
                    vertices.Add((i, j));
                    int height = Grid[i, j];

                    if (i > 0 && Grid[i - 1, j] <= height + 1)
                    {
                        edges.Add(new ((i, j), (i - 1, j)));
                    }

                    if (i < gridHeight - 1 && Grid[i + 1, j] <= height + 1)
                    {
                        edges.Add(new ((i, j), (i + 1, j)));
                    }

                    if (j > 0 && Grid[i, j - 1] <= height + 1)
                    {
                        edges.Add(new ((i, j), (i, j - 1)));
                    }

                    if (j < gridWidth - 1 && Grid[i, j + 1] <= height + 1)
                    {
                        edges.Add(new ((i, j), (i, j + 1)));
                    }
                }
            }

            return new Graph<(int, int)>(vertices, edges);
        }
    }

    public Map Parse()
    {
        IList<string> lines = this.Input!.SplitByNewline().ToList();
        var alphabet = "abcdefghijklmnopqrstuvwxyz";

        var grid = new int[lines.Count, lines[0].Length];
        (int, int)? start = null;
        (int, int)? end = null;
        List<(int, int)> otherStarts = new();

        for (int i = 0; i < lines.Count; i++)
        {
            string line = lines[i];
            for (int j = 0; j < line.Length; j++)
            {
                char c = line[j];

                int height;

                if (c == 'S')
                {
                    height = alphabet.IndexOf('a');
                    start = (i, j);
                }
                else if (c == 'a')
                {
                    height = alphabet.IndexOf("a");
                    otherStarts.Add((i, j));
                }
                else if (c == 'E')
                {
                    height = alphabet.IndexOf("z");
                    end = (i , j);
                }
                else
                {
                    height = alphabet.IndexOf(c);
                }

                grid[i, j] = height;
            }
        }

        if (start == null || end == null)
        {
            throw new Exception("Missing start or end");
        }

        return new Map(grid, start.Value, end.Value, otherStarts);
    }

    public class Graph<T> where T : notnull
    {
        public Graph(IEnumerable<T> vertices, IEnumerable<Tuple<T, T>> edges)
        {
            foreach (var vertex in vertices)
                AddVertex(vertex);

            foreach (var edge in edges)
                AddEdge(edge);
        }

        public Dictionary<T, HashSet<T>> AdjacencyList { get; } = new Dictionary<T, HashSet<T>>();

        public void AddVertex(T vertex)
        {
            AdjacencyList[vertex] = new HashSet<T>();
        }

        public void AddEdge(Tuple<T, T> edge)
        {
            AdjacencyList[edge.Item1].Add(edge.Item2);
        }
    }

    public Func<T, IEnumerable<T>?> BFSShortestPathFunction<T>(Graph<T> graph, T start) where T : notnull
    {
        var previous = new Dictionary<T, T>();

        var queue = new Queue<T>();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var vertex = queue.Dequeue();
            foreach (var neighbor in graph.AdjacencyList[vertex])
            {
                if (previous.ContainsKey(neighbor))
                    continue;

                previous[neighbor] = vertex;
                queue.Enqueue(neighbor);
            }
        }

        IEnumerable<T>? shortestPath(T end)
        {
            if (!previous.ContainsKey(end))
            {
                return null;
            }

            var path = new List<T> { };

            var current = end;
            while (!current.Equals(start))
            {
                path.Add(current);
                current = previous[current];
            };

            path.Add(start);
            path.Reverse();

            return path;
        }

        return shortestPath;
    }

    protected override string SolvePartOne()
    {
        var map = Parse();

        var graph = map.ToGraph();

        var startVertex = map.Start;
        var shortestPathFunc = BFSShortestPathFunction(graph, startVertex);
        var shortestPath = shortestPathFunc(map.End);

        if (shortestPath == null)
        {
            throw new Exception("No path found");
        }

        var steps = shortestPath.Count() - 1;

        return steps.ToString();
    }

    protected override string SolvePartTwo()
    {
        var map = Parse();

        var graph = map.ToGraph();

        int fewestSteps = int.MaxValue;

        foreach (var startVertex in map.OtherStarts)
        {
            var shortestPathFunc = BFSShortestPathFunction(graph, startVertex);

            var shortestPath = shortestPathFunc(map.End);

            if (shortestPath == null)
            {
                // No path found
                continue;
            }

            var steps = shortestPath.Count() - 1;
            if (steps < fewestSteps) fewestSteps = steps;
        }

        return fewestSteps.ToString();
    }
}
