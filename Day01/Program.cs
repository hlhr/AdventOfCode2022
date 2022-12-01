var elvesTop3 = File.ReadAllLines("input.txt").Aggregate(new List<int> { 0 }, (elves, line) =>
{
    if (line == string.Empty)
    {
        elves.Add(0);
    }
    else
    {
        elves[^1] += int.Parse(line);
    }

    return elves;
}).OrderDescending().Take(3).ToList();

Console.WriteLine($"Top elf: {elvesTop3.First()} Calories");
Console.WriteLine($"Sum of top 3: {elvesTop3.Sum()} Calories");