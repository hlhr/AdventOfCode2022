List<(int Index, long Value)> GetInput(long key)
{
    return File.ReadLines("input.txt")
        .Select(long.Parse)
        .Select((i, index) => (index, i * key))
        .ToList();
}

long GetAnswer(List<(int Index, long Value)> input, int loops)
{
    var inputLength = input.Count - 1;
    var updated = new List<(int Index, long Value)>(input);
    for (var i = 0; i < loops; i++)
    {
        foreach (var (index, value) in input)
        {
            var oldPosition = updated.IndexOf((index, value));
            var newPosition = (int)((oldPosition + value) % inputLength);
            if (newPosition < 0)
            {
                newPosition += inputLength;
            }

            updated.RemoveAt(oldPosition);
            updated.Insert(newPosition, (index, value));
        }
    }

    var indexOfZero = updated.FindIndex(i => i.Value == 0);

    var one = updated[(indexOfZero + 1000) % inputLength].Value;
    var two = updated[(indexOfZero + 2000) % inputLength].Value;
    var three = updated[(indexOfZero + 3000) % inputLength].Value;

    return one + two + three;
}

var inputPart1 = GetInput(1);
var part1 = GetAnswer(inputPart1, 1);
Console.WriteLine(part1);

var inputPart2 = GetInput(811589153);
var part2 = GetAnswer(inputPart2, 10);
Console.WriteLine(part2);