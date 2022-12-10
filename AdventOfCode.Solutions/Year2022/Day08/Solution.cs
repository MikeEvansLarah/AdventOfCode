namespace AdventOfCode.Solutions.Year2022.Day08;

class Solution : SolutionBase
{
    public Solution() : base(08, 2022, "Treetop Tree House") { }

    public static bool IsVisible(int[,] grid, int row, int col)
    {
        // On edges
        if (row == 0 || col == 0) return true;
        if (row == grid.GetLength(0) - 1) return true;
        if (col == grid.GetLength(1) - 1) return true;

        int height = grid[row,col];

        // Visible from left
        bool visibleFromLeft = true;
        for (int c = 0; c < col; c++)
        {
            if (grid[row,c] >= height) { visibleFromLeft = false; break; }
        }

        // Visible from right
        bool visibleFromRight = true;
        for (int c = col + 1; c < grid.GetLength(1); c++)
        {
            if (grid[row,c] >= height) { visibleFromRight = false; break; }
        }

        // Visible from top
        bool visibleFromTop = true;
        for (int r = 0; r < row; r++)
        {
            if (grid[r,col] >= height) { visibleFromTop = false; break; }
        }

        // Visible from bottom
        bool visibleFromBottom= true;
        for (int r = row + 1; r < grid.GetLength(0); r++)
        {
            if (grid[r,col] >= height) { visibleFromBottom = false; break; }
        }

        return visibleFromLeft || visibleFromRight || visibleFromTop || visibleFromBottom;
    }

    public static int ScenicScore(int[,] grid, int row, int col)
    {
        int height = grid[row, col];

        // Looking left
        int leftScore = 0;
        for (int c = col - 1; c >= 0; c--)
        {
            leftScore++;
            if (grid[row, c] >= height) {  break; }
        }

        // Looking right
        int rightScore = 0;
        for (int c = col + 1; c < grid.GetLength(1); c++)
        {
            rightScore++;
            if (grid[row, c] >= height) { break; }
        }

        // Looking up
        int upScore = 0;
        for (int r = row - 1; r >= 0; r--)
        {
            upScore++;
            if (grid[r, col] >= height) { break; }
        }

        // Looking down
        int downScore = 0;
        for (int r = row + 1; r < grid.GetLength(0); r++)
        {
            downScore++;
            if (grid[r, col] >= height) { break; }
        }

        return leftScore * rightScore * upScore * downScore;
    }

    protected override string SolvePartOne()
    {
        int[,] grid = ParseGrid();

        int totalVisible = 0;
        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int col = 0; col < grid.GetLength(1); col++)
            {
                if (IsVisible(grid, row, col)) totalVisible++;
            }
        }

        return totalVisible.ToString();
    }

    protected override string SolvePartTwo()
    {
        int[,] grid = ParseGrid();

        int bestScenicScore = 0;
        for(int row = 0; row < grid.GetLength(0); row++)
        {
            for (int col = 0; col < grid.GetLength(1); col++)
            {
                int scenicScore = ScenicScore(grid, row, col);
                if (scenicScore > bestScenicScore) bestScenicScore = scenicScore;
            }
        }

        return bestScenicScore.ToString();
    }

    private int[,] ParseGrid()
    {
        var splitLines = this.Input!.SplitByNewline();
        int[,] grid = new int[splitLines.Length, splitLines[0].Length];

        for (int row = 0; row < splitLines.Length; row++)
        {
            string? line = splitLines[row];
            for (int col = 0; col < line.Length; col++)
            {
                int height = int.Parse(line[col].ToString());
                grid[row, col] = height;
            }
        }

        return grid;
    }
}
