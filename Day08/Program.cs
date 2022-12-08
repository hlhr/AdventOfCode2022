int[][] ReadGrid(IEnumerable<string> lines)
{
    return lines
        .Select(line =>
            line.Chunk(1).Select(number => int.Parse(number)).ToArray())
        .ToArray();
}

bool IsVisibleInColumn(IReadOnlyList<int[]> grid, int height, int rowIndex, int startColumnIndex, int endColumnIndex)
{
    for (var columnIndex = startColumnIndex; columnIndex < endColumnIndex; columnIndex++)
    {
        if (grid[rowIndex][columnIndex] >= height)
        {
            return false;
        }
    }
    return true;
}

bool IsVisibleInRow(IReadOnlyList<int[]> grid, int height, int columnIndex, int startRowIndex, int endRowIndex)
{
    for (var rowIndex = startRowIndex; rowIndex < endRowIndex; rowIndex++)
    {
        if (grid[rowIndex][columnIndex] >= height)
        {
            return false;
        }
    }

    return true;
}

int GetAnswer1(IReadOnlyList<int[]> grid)
{
    var visibleCounter = (grid.Count + grid.First().Length) * 2 - 4;

    for (var rowIndex = 1; rowIndex < grid.Count - 1; rowIndex++)
    {
        for (var columnIndex = 1; columnIndex < grid[rowIndex].Length - 1; columnIndex++)
        {
            if (IsVisibleInColumn(grid, grid[rowIndex][columnIndex], rowIndex, 0, columnIndex)
                || IsVisibleInColumn(grid, grid[rowIndex][columnIndex], rowIndex, columnIndex + 1, grid[rowIndex].Length)
                || IsVisibleInRow(grid, grid[rowIndex][columnIndex], columnIndex, 0, rowIndex)
                || IsVisibleInRow(grid, grid[rowIndex][columnIndex], columnIndex, rowIndex + 1, grid.Count))
            {
                visibleCounter++;
            }
        }
    }

    return visibleCounter;
}

var input = File.ReadLines("input.txt").ToList();
var grid = ReadGrid(input);

Console.WriteLine($"How many trees are visible from outside the grid? {GetAnswer1(grid)}");