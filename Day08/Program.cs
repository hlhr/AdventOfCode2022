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

int GetAnswer2(IReadOnlyList<int[]> grid)
{
    var maxScore = 0;

    for (var rowIndex = 0; rowIndex < grid.Count; rowIndex++) {
        for (var columnIndex = 0; columnIndex < grid[rowIndex].Length; columnIndex++)
        {
            var height = grid[rowIndex][columnIndex];
            var score = 1;

            // top
            var multiple = 0;
            for (var currentRowIndex = rowIndex - 1; currentRowIndex >= 0; currentRowIndex--)
            {
                multiple++;
                if (grid[currentRowIndex][columnIndex] >= height)
                {
                    break;
                }
            }

            score *= multiple;

            // right
            multiple = 0;
            for (var currentColumnIndex = columnIndex - 1; currentColumnIndex >= 0; currentColumnIndex--)
            {
                multiple++;
                if (grid[rowIndex][currentColumnIndex] >= height)
                {
                    break;
                }
            }

            score *= multiple;

            // bottom
            multiple = 0;
            for (var currentRowIndex = rowIndex + 1; currentRowIndex < grid.Count; currentRowIndex++)
            {
                multiple++;
                if (grid[currentRowIndex][columnIndex] >= height)
                {
                    break;
                }
            }

            score *= multiple;

            // left
            multiple = 0;
            for (var currentColumnIndex = columnIndex + 1; currentColumnIndex < grid[rowIndex].Length; currentColumnIndex++)
            {
                multiple++;
                if (grid[rowIndex][currentColumnIndex] >= height)
                {
                    break;
                }
            }

            score *= multiple;

            if (score > maxScore)
            {
                maxScore = score;
            }
        }
    }
    return maxScore;
}

var input = File.ReadLines("input.txt").ToList();
var grid = ReadGrid(input);

Console.WriteLine($"How many trees are visible from outside tree grid? {GetAnswer1(grid)}");
Console.WriteLine($"What is the highest scenic score possible for any tree? {GetAnswer2(grid)}");