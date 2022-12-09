using System.Drawing;

var moves = File
    .ReadLines("input.txt")
    .Select(move => (Direction: move[0], Steps: int.Parse(move[2..])))
    .ToList();

var pointsTraveledByHead = moves.Aggregate(
    new List<Point> { new(0, 0) }, (traveled, move) =>
    {
        for (var step = 0; step < move.Steps; step++)
        {
            var start = traveled.Last();
            var to = move.Direction switch
            {
                'L' => start with { X = start.X - 1 },
                'R' => start with { X = start.X + 1 },
                'U' => start with { Y = start.Y + 1 },
                'D' => start with { Y = start.Y - 1 },
                _ => throw new ArgumentOutOfRangeException()
            };
            traveled.Add(to);
        }

        return traveled;
    });

var pointsTraveledByTail = pointsTraveledByHead.Aggregate(new List<Point> {new (0, 0)}, (traveled, headMove ) =>
{
    var current = traveled.Last();
    var stepX = headMove.X - current.X;
    var stepY = headMove.Y - current.Y;

    if (Math.Abs(stepX) <= 1 && Math.Abs(stepY) <= 1)
    {
        return traveled;
    }

    var next = new Point(current.X + Math.Sign(stepX), current.Y + Math.Sign(stepY));

    traveled.Add(next);
    return traveled;
});

// At least one visit
var tailVisitCount = pointsTraveledByTail.Distinct().Count();

Console.WriteLine($"How many positions does the tail of the rope visit at least once? {tailVisitCount}");