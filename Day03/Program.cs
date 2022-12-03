int GetPriority(char itemType)
{
    return char.ToUpper(itemType) - (char.IsUpper(itemType) ? 38 : 64);
}

var input = File.ReadAllLines("input.txt").Where(line => !string.IsNullOrEmpty(line)).ToList();

var sumOfPriorities = input
    .Sum(line =>
    {
        var middle = line.Length / 2;
        var firstCompartment = line[..middle];
        var secondCompartment = line[middle..];

        return firstCompartment
            .Intersect(secondCompartment)
            .Sum(GetPriority);
    });

var sumOfGroupPriorities = input
    .Chunk(3)
    .Sum(group => group[0]
        .Intersect(group.ElementAtOrDefault(1) ?? string.Empty)
        .Intersect(group.ElementAtOrDefault(2) ?? string.Empty)
        .Sum(GetPriority));

Console.WriteLine($"Sum of the priorities: {sumOfPriorities}");
Console.WriteLine($"Sum of the groupt priorities: {sumOfGroupPriorities}");