var inputLines = File.ReadLinesAsync("input.txt");

var currentElfCalories = 0;

var elves = new List<Elf>();

await foreach (var line in inputLines)
{
    if (string.IsNullOrEmpty(line))
    {
        var elf = new Elf(elves.Count + 1, currentElfCalories);
        elves.Add(elf);

        currentElfCalories = 0;

        continue;
    }

    currentElfCalories += int.Parse(line);
}

var elvesCharts = elves.OrderDescending().ToList();

Console.WriteLine($"Heavy loader: #{elvesCharts.First().Number}: {elvesCharts.First().Calories} Calories");
Console.WriteLine($"Sum of top 3: {elvesCharts.Take(3).Sum(elf => elf.Calories)}");

internal record Elf(int Number, int Calories) : IComparable<Elf>
{
    public readonly int Number = Number;
    public readonly int Calories = Calories;

    public int CompareTo(Elf? other)
    {
        return Calories.CompareTo(other?.Calories);
    }
}