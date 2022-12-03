internal class Program
{
    private static void Main(string[] args)
    { 
        var lines = File.ReadLines("input1.txt");
        var result1 = lines.Select(l => {
            var pack1 = l.Take(l.Length / 2);
            var pack2 = l.Skip(l.Length / 2);
            return GetValue(pack1.Intersect(pack2).Single());
        }).Sum();
        Console.WriteLine(result1);

        var result2 = ChunkList(lines.Select(l => l.ToCharArray()), 3)
            .Select(chunk => GetValue(chunk[0].Intersect(chunk[1]).Intersect(chunk[2]).Single()))
            .Sum();
        Console.WriteLine(result2);
    }

    public static int GetValue(char c) 
    {
        if (Char.IsLower(c)) {
            return (int)c - (int)'a' + 1;
        } else {
            return (int)c - (int)'A' + 27;
        }
    }
    public static List<List<T>> ChunkList<T>(IEnumerable<T> data, int size)
    {
        return data
            .Select((x, i) => new { Index = i, Value = x })
            .GroupBy(x => x.Index / size)
            .Select(x => x.Select(v => v.Value).ToList())
            .ToList();
    }
}