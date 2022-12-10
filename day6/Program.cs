internal class Program
{
    private static void Main(string[] args)
    {
        var packet = File.ReadLines("input1.txt").Single();
        Console.WriteLine(GetPacketIndex(packet, 4));
        Console.WriteLine(GetPacketIndex(packet, 14));
    }

    public static int GetPacketIndex(string packet, int markerLength) 
    {
        for (var i = 0; i < packet.Length - markerLength - 1; i++){
            if(packet.Skip(i).Take(markerLength).Distinct().Count() == markerLength) {
                return i + markerLength;
            }
        }
        return -1;
    }
}
