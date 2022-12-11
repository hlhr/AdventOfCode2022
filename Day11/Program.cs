var monkeys = File.ReadAllLines("input.txt")
    .Where(line => !string.IsNullOrWhiteSpace(line))
    .Chunk(6)
    .Select(monkeyDefinition =>
    {
        var number = int.Parse(monkeyDefinition[0][7..^1]);
        var items = monkeyDefinition[1][18..]
            .Split(',', StringSplitOptions.TrimEntries)
            .Select(int.Parse)
            .ToList();
        var throwCheckDivisibleBy = int.Parse(monkeyDefinition[3][21..]);
        var positivCheckThrowTo = int.Parse(monkeyDefinition[4][29..]);
        var negativCheckThrowTo = int.Parse(monkeyDefinition[5][29..]);

        var operation = monkeyDefinition[2][19..];
        Func<int, int> inspection;

        if (operation == "old * old")
        {
            inspection = (old) => old * old;
        }
        else if (operation.StartsWith("old +"))
        {
            inspection = (old) => old + int.Parse(operation[6..]);
        }
        else if (operation.StartsWith("old *"))
        {
            inspection = (old) => old * int.Parse(operation[6..]);
        }
        else
        {
            throw new ArgumentOutOfRangeException();
        }

        return new Monkey
        {
            Number = number,
            Items = items,
            Inspection = inspection,
            ThrowCheckDivisibleBy = throwCheckDivisibleBy,
            PositivCheckThrowTo = positivCheckThrowTo,
            NegativCheckThrowTo = negativCheckThrowTo
        };
    }).ToList();

const int roundsToPlay = 20;

for (var round = 0; round < roundsToPlay; round++)
{
    foreach (var monkey in monkeys)
    {
        foreach (var item in monkey.Items)
        {
            var newItem = monkey.Inspection(item);
            var newWorryLevel = newItem / 3;
            var target = newWorryLevel % monkey.ThrowCheckDivisibleBy == 0
                ? monkey.PositivCheckThrowTo
                : monkey.NegativCheckThrowTo;
            monkeys[target].Items.Add(newWorryLevel);

            monkey.InspectedItemCount++;
        }

        monkey.Items = new List<int>();
    }
}

var topMonkeys = monkeys.Select(monkey => monkey.InspectedItemCount).OrderDescending().ToList();
var monkeyBusiness = topMonkeys[0] * topMonkeys[1];
Console.WriteLine($"What is the level of monkey business after 20 rounds of stuff-slinging simian shenanigans? {monkeyBusiness}");

internal class Monkey
{
    public required int Number { get; init; }

    public required List<int> Items { get; set; }

    public int InspectedItemCount { get; set; } = 0;

    public required Func<int, int> Inspection { get; init; }

    public required int ThrowCheckDivisibleBy { get; init; }

    public required int PositivCheckThrowTo { get; init; }

    public required int NegativCheckThrowTo { get; init; }
}