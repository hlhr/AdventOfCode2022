using System.Drawing;

List<Point> Follow(List<Point> traveled, Point moveToFollow)
{
    var current = traveled[^1];
    var stepX = moveToFollow.X - current.X;
    var stepY = moveToFollow.Y - current.Y;

    if (Math.Abs(stepX) <= 1 && Math.Abs(stepY) <= 1)
    {
        return traveled;
    }

    var next = new Point(current.X + Math.Sign(stepX), current.Y + Math.Sign(stepY));

    traveled.Add(next);
    return traveled;
}

var pointsTraveledByHead =  File
    .ReadLines("input.txt")
    .Select(move => (Direction: move[0], Steps: int.Parse(move[2..])))
    .Aggregate(
    new List<Point> { new(0, 0) }, (traveled, move) =>
    {
        for (var step = 0; step < move.Steps; step++)
        {
            var start = traveled[^1];
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

var tailVisitCount = pointsTraveledByHead
    .Aggregate(new List<Point> {new (0, 0)}, Follow)
    .Distinct()
    .Count();
Console.WriteLine($"Part 1: {tailVisitCount}");

var tailMoves = new List<List<Point>>(9)
{
    new() { new Point(0, 0) },
    new() { new Point(0, 0) },
    new() { new Point(0, 0) },
    new() { new Point(0, 0) },
    new() { new Point(0, 0) },
    new() { new Point(0, 0) },
    new() { new Point(0, 0) },
    new() { new Point(0, 0) },
    new() { new Point(0, 0) },
};

var visitByTail = new List<Point>();

foreach (var headMove in pointsTraveledByHead)
{
    tailMoves[0] = Follow(tailMoves[0], headMove);

    for (var tailIndex = 1; tailIndex < tailMoves.Count; tailIndex++)
    {
        tailMoves[tailIndex] = Follow(tailMoves[tailIndex], tailMoves[tailIndex - 1].Last());
    }

    visitByTail.Add(tailMoves[^1][^1]);
}


var tailsVisitCount = visitByTail.Distinct().Count();
Console.WriteLine($"Part 2: {tailsVisitCount}");