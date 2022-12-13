using System.Text.Json;

IPacket GetPackets(JsonElement element)
{
    return element.ValueKind == JsonValueKind.Number
        ? new ValuePacket(element.GetInt32())
        : new ListPacket(element.EnumerateArray().Select(GetPackets).ToList());
}

int ComparePackets(IPacket leftPacket, IPacket rightPacket)
{
    return (leftPacket, rightPacket) switch
    {
        (not null, null) => 1,
        (null, not null) => -1,
        (ValuePacket left, ValuePacket right) => CompareValues(left, right),
        (ValuePacket left, ListPacket right) => ComparePackets(new ListPacket(new List<IPacket> { left }), right),
        (ListPacket left, ValuePacket right) => ComparePackets(left, new ListPacket(new List<IPacket> { right })),
        (ListPacket left, ListPacket right) => CompareLists(left, right),
        _ => throw new ArgumentOutOfRangeException()
    };
}

int CompareLists(ListPacket left, ListPacket right)
{
    return left.Packets.Zip(right.Packets)
        .Select(p => ComparePackets(p.First, p.Second))
        .FirstOrDefault(c => c != 0, left.Packets.Count - right.Packets.Count);
}

int CompareValues(ValuePacket left, ValuePacket right)
{
    var result = Comparer<int>.Default.Compare(left.Value, right.Value);
    return result;
}

var answer1 = 0;

var chunks = File.ReadLines("input.txt").Where(line => !string.IsNullOrEmpty(line)).Chunk(2).ToList();
for (var i = 0; i < chunks.Count; i++)
{
    var leftElement = JsonSerializer.Deserialize<JsonElement>(chunks[i][0]);
    var rightElement = JsonSerializer.Deserialize<JsonElement>(chunks[i][1]);

    var left = GetPackets(leftElement);
    var right = GetPackets(rightElement);
    var compare = ComparePackets(left, right);

    if (compare < 0)
    {
        answer1 += i + 1;
    }
}
Console.WriteLine($"Part 1: {answer1}");

internal interface IPacket { };
internal record ValuePacket(int Value) : IPacket;
internal record ListPacket(List<IPacket> Packets) : IPacket;