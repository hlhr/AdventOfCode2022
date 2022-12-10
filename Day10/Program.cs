var program = File
    .ReadLines("input.txt")
    .Aggregate(new List<int>(), (signals, line) =>
    {
        var latest = signals.Count == 0 ? 1 : signals[^1];
        signals.Add(latest);

        if (line != "noop")
        {
            var value = int.Parse(line[5..]);
            signals.Add(latest + value);
        }

        return signals;
    });

var cyclesForStrength =  new List<int> {20, 60, 100, 140, 180, 220};
var cyclesStrength = cyclesForStrength.Sum(cycle => cycle *  program[cycle - 2]);
Console.WriteLine(cyclesStrength);

var screen = string.Empty;
for (var i = 0; i < program.Count; i++)
{
    var spriteMiddle = i == 0 ? 1 : program[i - 1];
    var screenColumn = (i + 1 - 1) % 40;

    screen += Math.Abs(spriteMiddle - screenColumn) < 2 ? "X" : " ";

    if (screenColumn == 39)
    {
        screen += Environment.NewLine;
    }
}
Console.WriteLine(screen);