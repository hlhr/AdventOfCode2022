var elvesPairs = File.ReadAllLines("input.txt")
    .Where(line => !string.IsNullOrEmpty(line))
    .Select(line =>
        {
            return line
                .Split(',')
                .Select(sections => sections.Split('-')
                    .Select(int.Parse)
                    .ToArray())
                .ToArray();
        }
    )
    .ToList();

var fullyContains = elvesPairs
    .Count(elvesPair => (elvesPair[0][0] <= elvesPair[1][0] && elvesPair[0][1] >= elvesPair[1][1]) ||
                        (elvesPair[1][0] <= elvesPair[0][0] && elvesPair[1][1] >= elvesPair[0][1]));

var overlapping = elvesPairs
    .Count(elvesPair => elvesPair[0][1] >= elvesPair[1][0] && elvesPair[1][1] >= elvesPair[0][0]);

Console.WriteLine($"In how many assignment pairs does one range fully contain the other? {fullyContains}");
Console.WriteLine($"In how many assignment pairs do the ranges overlap?? {overlapping}");