internal class Program
{
    private static void Main(string[] args)
    {
        var lines = File.ReadLines("input1.txt");
        var assignmentPairs = lines.Select(ParseLine);
        var result1 = assignmentPairs
            .Where(x => x.Item1.Intersect(x.Item2).Count() == Math.Min(x.Item1.Length, x.Item2.Length))
            .Count();
        Console.WriteLine(result1);
        var result2 = assignmentPairs
            .Where(x => x.Item1.Intersect(x.Item2).Any())
            .Count();
        Console.WriteLine(result2);
    }

    public static (int[], int[]) ParseLine(string line) 
    {
        var assignments = line.Split(',')
            .Select(x => {
                var range = x.Split('-')
                    .Select(int.Parse)
                    .ToArray();
                return Enumerable.Range(range[0], range[1] - range[0] + 1).ToArray();
            }).ToList();
        return (assignments[0], assignments[1]);
    }
}