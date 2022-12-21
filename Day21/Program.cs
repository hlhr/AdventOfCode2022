var results = new Dictionary<string, long>();

var openForCalculation = new List<(string name, string left, string operation, string right)>();

foreach (var line in File.ReadLines("input.txt"))
{
	var monkeyName = line[..4];
	var monkeyCommand = line[6..];

	if (long.TryParse(monkeyCommand, out var number))
	{
		results.Add(monkeyName, number);
	}
	else
	{
		var split = monkeyCommand.Split(' ');
		openForCalculation.Add((monkeyName, split[0], split[1], split[2]));
	}
}

while (openForCalculation.Any())
{
	var toCalculate = openForCalculation.First(w => results.ContainsKey(w.left) && results.ContainsKey(w.right));
	openForCalculation.Remove(toCalculate);

	results[toCalculate.name] = toCalculate.operation switch
	{
		"+" => results[toCalculate.left] + results[toCalculate.right],
		"-" => results[toCalculate.left] - results[toCalculate.right],
		"*" => results[toCalculate.left] * results[toCalculate.right],
		"/" => results[toCalculate.left] / results[toCalculate.right],
		_ => throw new ArgumentOutOfRangeException()
	};
}

Console.WriteLine($"Answer 1: {results["root"]}");