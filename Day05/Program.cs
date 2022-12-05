using System.Text.RegularExpressions;

var input = File.ReadLines("input.txt").ToList();

var cargoDefinitions = input.TakeWhile(line => !char.IsNumber(line[1])).Reverse().ToList();

var cargoWidth = int.Parse(input.Skip(cargoDefinitions.Count).First().Last().ToString());

var cargo9000 = new Stack<char>[cargoWidth];
var cargo9001 = new Stack<char>[cargoWidth];

foreach (var chunks in cargoDefinitions.Select(line => line.Chunk(4).ToList()))
{
    for (var i = 0; i < chunks.Count; i++)
    {
        if (char.IsLetter(chunks[i][1]))
        {
            cargo9000[i] ??= new Stack<char>();
            cargo9000[i].Push(chunks[i][1]);

            cargo9001[i] ??= new Stack<char>();
            cargo9001[i].Push(chunks[i][1]);
        }
    }
}

foreach (var line in input.Skip(cargoDefinitions.Count + 2))
{
    var moves = Regex
        .Split(line, @"[^0-9]+", RegexOptions.IgnorePatternWhitespace)
        .Where(s => !string.IsNullOrEmpty(s))
        .Select(int.Parse)
        .ToArray();

    var quantity = moves[0];
    var from = moves[1] - 1;
    var to = moves[2] - 1;

    var stackFor9001 = new Stack<char>(quantity);

    for (var i = 0; i < quantity; i++)
    {
        cargo9000[to].Push(cargo9000[from].Pop());
        stackFor9001.Push(cargo9001[from].Pop());
    }

    for (var i = 0; i < quantity; i++)
        cargo9001[to].Push(stackFor9001.Pop());
}

var topCrates9000 = string.Concat(cargo9000.Select(crates => crates.Peek()).ToArray());
var topCrates9001 = string.Concat(cargo9001.Select(crates => crates.Peek()).ToArray());

Console.WriteLine("After the rearrangement procedure completes, what crate ends up on top of each stack?");
Console.WriteLine($"CrateMover 9000: {topCrates9000}");
Console.WriteLine($"CrateMover 9001: {topCrates9001}");