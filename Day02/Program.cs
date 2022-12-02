Shapes GetShape(string shapeAsString)
{
    return shapeAsString switch
    {
        "A" or "X" => Shapes.Rock,
        "B" or "Y" => Shapes.Paper,
        "C" or "Z" => Shapes.Scissors,
        _ => throw new ArgumentOutOfRangeException(nameof(shapeAsString), shapeAsString, null)
    };
}

Outcome GetWantedOutcome(string wantedOutcome)
{
    return wantedOutcome switch
    {
        "X" => Outcome.Lose,
        "Y" => Outcome.Draw,
        "Z" => Outcome.Win,
        _ => throw new ArgumentOutOfRangeException(nameof(wantedOutcome), wantedOutcome, null)
    };
}

Outcome GetOutcome(Shapes choice, Shapes opponent)
{
    if (choice == opponent)
    {
        return Outcome.Draw;
    }

    // Rock defeats Scissors, Scissors defeats Paper, and Paper defeats Rock
    if ((choice == Shapes.Rock && opponent == Shapes.Scissors)
        || (choice == Shapes.Scissors && opponent == Shapes.Paper)
        || (choice == Shapes.Paper && opponent == Shapes.Rock))
    {
        return Outcome.Win;
    }

    return Outcome.Lose;
}


Shapes GetShapeForOutcome(Shapes opponent, Outcome wantedOutcome)
{
    return wantedOutcome switch
    {
        Outcome.Win => opponent switch
        {
            Shapes.Rock => Shapes.Paper,
            Shapes.Paper => Shapes.Scissors,
            Shapes.Scissors => Shapes.Rock,
            _ => throw new ArgumentOutOfRangeException(nameof(opponent), opponent, null)
        },
        Outcome.Lose => opponent switch
        {
            Shapes.Rock => Shapes.Scissors,
            Shapes.Paper => Shapes.Rock,
            Shapes.Scissors => Shapes.Paper,
            _ => throw new ArgumentOutOfRangeException(nameof(opponent), opponent, null)
        },
        _ => opponent
    };
}

var input = File.ReadAllLines("input.txt")
    .Where(line => !string.IsNullOrEmpty(line))
    .Select(line =>
    {
        var splitter = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return (FirstColumn: splitter[0], SecondColumn: splitter[1]);
    })
    .ToList();

var totalPart1 = input
    .Sum(row =>
    {
        var opponent = GetShape(row.FirstColumn);
        var choice = GetShape(row.SecondColumn);

        return (int)GetOutcome(choice, opponent) + (int)choice;
    });

var totalPart2 = input
    .Sum(row =>
    {
        var opponent = GetShape(row.FirstColumn);
        var wantedOutcome = GetWantedOutcome(row.SecondColumn);
        var choice = GetShapeForOutcome(opponent, wantedOutcome);

        return (int)GetOutcome(choice, opponent) + (int)choice;
    });

Console.WriteLine($"Total Part 1: {totalPart1}");
Console.WriteLine($"Total Part 2: {totalPart2}");

internal enum Shapes
{
    Rock = 1,
    Paper = 2,
    Scissors = 3,
}

internal enum Outcome
{
    Win = 6,
    Lose = 0,
    Draw = 3,
}