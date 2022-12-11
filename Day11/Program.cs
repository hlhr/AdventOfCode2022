List<Monkey> ParseMonkeys()
{
    return File.ReadAllLines("input.txt")
        .Where(line => !string.IsNullOrWhiteSpace(line))
        .Chunk(6)
        .Select(monkeyDefinition =>
        {
            var items = monkeyDefinition[1][18..]
                .Split(',', StringSplitOptions.TrimEntries)
                .Select(long.Parse)
                .ToList();
            var throwCheckDivisibleBy = int.Parse(monkeyDefinition[3][21..]);
            var positivCheckThrowTo = int.Parse(monkeyDefinition[4][29..]);
            var negativCheckThrowTo = int.Parse(monkeyDefinition[5][29..]);

            var operation = monkeyDefinition[2][19..];
            Func<long, long> inspection;

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
                Items = items,
                Inspection = inspection,
                ThrowCheckDivisibleBy = throwCheckDivisibleBy,
                PositivCheckThrowTo = positivCheckThrowTo,
                NegativCheckThrowTo = negativCheckThrowTo
            };
        }).ToList();
}

void Run(int roundsToPlay, List<Monkey> monkeys, Func<long, long> updateWorryLevel)
{
    for (var round = 0; round < roundsToPlay; round++)
    {
        foreach (var monkey in monkeys)
        {
            foreach (var item in monkey.Items)
            {
                var newItem = monkey.Inspection(item);
                var newWorryLevel = updateWorryLevel(newItem);
                var target = newWorryLevel % monkey.ThrowCheckDivisibleBy == 0
                    ? monkey.PositivCheckThrowTo
                    : monkey.NegativCheckThrowTo;
                monkeys[target].Items.Add(newWorryLevel);

                monkey.InspectedItemCount++;
            }

            monkey.Items.Clear();
        }
    }

    var topMonkeys = monkeys.Select(monkey => monkey.InspectedItemCount).OrderDescending().Take(2).ToList();
    var monkeyBusiness = topMonkeys[0] * topMonkeys[1];
    Console.WriteLine($"What is the level of monkey business after {roundsToPlay} rounds of stuff-slinging simian shenanigans? {monkeyBusiness}");
}

void Part1()
{
    var monkeys = ParseMonkeys();
    Run(20, monkeys, i => i / 3);
}

void Part2()
{
    var monkeys = ParseMonkeys();
    var mod = monkeys.Aggregate(1, (total, current) => total * current.ThrowCheckDivisibleBy);
    Run(10000, monkeys, i => i % mod);
}

Part1();
Part2();

internal class Monkey
{
    public required List<long> Items { get; init; }

    public long InspectedItemCount { get; set; }

    public required Func<long, long> Inspection { get; init; }

    public required int ThrowCheckDivisibleBy { get; init; }

    public required int PositivCheckThrowTo { get; init; }

    public required int NegativCheckThrowTo { get; init; }
}