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

var packets = File.ReadLines("input.txt")
    .Where(line => !string.IsNullOrEmpty(line))
    .Select(line => GetPackets(JsonSerializer.Deserialize<JsonElement>(line)))
    .ToList();


var answer1 = 0;
var packetPairs = packets.Chunk(2).ToList();
for (var i = 0; i < packetPairs.Count; i++)
{
    var compare = ComparePackets(packetPairs[i][0], packetPairs[i][1]);

    if (compare < 0)
    {
        answer1 += i + 1;
    }
}
Console.WriteLine($"Part 1: {answer1}");

var divider2 = GetPackets(JsonSerializer.Deserialize<JsonElement>("[[2]]"));
var divider6 = GetPackets(JsonSerializer.Deserialize<JsonElement>("[[6]]"));
packets.Add(divider2);
packets.Add(divider6);
var orderedPackets = packets.Order(Comparer<IPacket>.Create(ComparePackets)).ToList();

var divider2Index = orderedPackets.FindIndex(p => p == divider2) + 1;
var divider6Index = orderedPackets.FindIndex(p => p == divider6) + 1;
Console.WriteLine($"Part 2: {divider2Index * divider6Index}");

internal interface IPacket { };
internal record ValuePacket(int Value) : IPacket;
internal record ListPacket(List<IPacket> Packets) : IPacket;