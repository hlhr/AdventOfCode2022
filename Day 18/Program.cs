var cubes = File.ReadLines("input.txt")
    .Select(line =>
    {
        var parts = line.Split(',');
        return (X: int.Parse(parts[0]), Y: int.Parse(parts[1]), Z: int.Parse(parts[2]));
    })
    .ToList();

var connected = cubes.Aggregate(0, (seed, cube) =>
{
    if (cubes.Contains((cube.X + 1, cube.Y, cube.Z)))
    {
        seed++;
    }
    if (cubes.Contains((cube.X - 1, cube.Y, cube.Z)))
    {
        seed++;
    }

    if (cubes.Contains((cube.X, cube.Y + 1, cube.Z)))
    {
        seed++;
    }
    if (cubes.Contains((cube.X, cube.Y - 1, cube.Z)))
    {
        seed++;
    }

    if (cubes.Contains((cube.X, cube.Y, cube.Z + 1)))
    {
        seed++;
    }
    if (cubes.Contains((cube.X, cube.Y, cube.Z - 1)))
    {
        seed++;
    }

    return seed;
});

var part1 = cubes.Count * 6 - connected;

Console.WriteLine($"Part 1: {part1}");