int GetMarkerPosition(string input, int markerLength)
{
    for (var position = 0; position < input.Length; position++)
    {
        var marker = input.Substring(position, markerLength);

        if (marker.Distinct().Count() == markerLength)
        {
            return position + markerLength;
        }
    }

    return -1;
}

var signal = File.ReadAllText("input.txt");

Console.WriteLine("How many characters need to be processed before the first start-of-packet marker is detected?");
Console.WriteLine($"Marker 4: {GetMarkerPosition(signal, 4)}");
Console.WriteLine($"Marker 14: {GetMarkerPosition(signal, 14)}");
