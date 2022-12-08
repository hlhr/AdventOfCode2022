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

int GetScoreLeft(int[][] grid, int currentRowIndex, int currentColumnIndex)
{
    var current = grid[currentRowIndex][currentColumnIndex];
    var left = grid[currentRowIndex].Reverse().Take(currentColumnIndex).ToList();
    return left.TakeWhile(i => i < current).Count() + (current == left.Last() ? 1 : 0);
}

int GetScoreRight(int[][] grid, int currentRowIndex, int currentColumnIndex)
{
    var current = grid[currentRowIndex][currentColumnIndex];
    var right = grid[currentRowIndex].Skip(currentColumnIndex + 1).ToList();
    return right.TakeWhile(i => i < current).Count() + (current == right.Last() ? 1 : 0);
}

int GetScoreTop(int[][] grid, int currentRowIndex, int currentColumnIndex)
{
    var current = grid[currentRowIndex][currentColumnIndex];
    var top = grid.Select(i => i[currentColumnIndex]).Reverse().Take(currentRowIndex).ToList();
    return top.TakeWhile(i => i < current).Count() + (current == top.Last() ? 1 : 0);
}

int GetScoreBottom(int[][] grid, int currentRowIndex, int currentColumnIndex)
{
    var current = grid[currentRowIndex][currentColumnIndex];
    var bottom = grid.Select(i => i[currentColumnIndex]).Skip(currentRowIndex).ToList();
    return bottom.TakeWhile(i => i < current).Count() + (current == bottom.Last() ? 1 : 0);
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

int GetAnswer2(int[][] grid)
{
    var maxScore = 0;

    for (var rowIndex = 0; rowIndex < grid.Length; rowIndex++) {
        for (var columnIndex = 0; columnIndex < grid[rowIndex].Length; columnIndex++)
        {
            var height = grid[rowIndex][columnIndex];
            var product = 1;

            // top
            var multiple = 0;
            for (var i = rowIndex - 1; i >= 0; i--)
            {
                multiple++;
                if (grid[i][columnIndex] >= height)
                {
                    break;
                }
            }

            product *= multiple;

            // right
            multiple = 0;
            for (var i = columnIndex - 1; i >= 0; i--)
            {
                multiple++;
                if (grid[rowIndex][i] >= height)
                {
                    break;
                }
            }

            product *= multiple;

            // bottom
            multiple = 0;
            for (var i = rowIndex + 1; i < grid.Length; i++)
            {
                multiple++;
                if (grid[i][columnIndex] >= height)
                {
                    break;
                }
            }

            product *= multiple;

            // left
            multiple = 0;
            for (var i = columnIndex + 1; i < grid[rowIndex].Length; i++)
            {
                multiple++;
                if (grid[rowIndex][i] >= height)
                {
                    break;
                }
            }

            product *= multiple;

            if (product > maxScore)
            {
                maxScore = product;
            }
        }
    }
    return maxScore;
}

var input = File.ReadLines("input.txt").ToList();
var grid = ReadGrid(input);

Console.WriteLine($"How many trees are visible from outside tree grid? {GetAnswer1(grid)}");
Console.WriteLine($"What is the highest scenic score possible for any tree? {GetAnswer2(grid)}");