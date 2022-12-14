internal class Program
{
    private static void Main(string[] args)
    {
        var (elevations, start, end) = ReadInput("input.txt");
        var best = Enumerable.Range(0, elevations.Length)
            .Select(_ => Enumerable.Repeat(int.MaxValue, elevations[0].Length).ToArray())
            .ToArray();
        Process(elevations, best, end, start, 0);
        Console.WriteLine(best[start.Item1][start.Item2]);
        var allStartScores = new List<int>();
        for(var i = 0; i < elevations.Length; i++) {
            for (var j = 0; j < elevations[0].Length; j++) {
                if(elevations[i][j] == 0){
                    allStartScores.Add(best[i][j]);
                }
            }
        }
        Console.WriteLine(allStartScores.Min());
    }


    public static void Process(int[][] elevations, int[][] best, (int, int) location, (int, int) end, int count){
        if(count >= best[location.Item1][location.Item2]) {
            return;
        } 
        best[location.Item1][location.Item2] = count;
        var currentEl = elevations[location.Item1][location.Item2];
        if(location.Item1 > 0 && elevations[location.Item1 - 1][location.Item2] - currentEl >= -1) {
            Process(elevations, best, (location.Item1 - 1, location.Item2), end, count + 1);
        }
        if(location.Item1 < elevations.Length - 1 && elevations[location.Item1 + 1][location.Item2] - currentEl >= -1) {
            Process(elevations, best, (location.Item1 + 1, location.Item2), end, count + 1);
        }
        if(location.Item2 > 0 && elevations[location.Item1][location.Item2 - 1] - currentEl >= -1) {
            Process(elevations, best, (location.Item1, location.Item2 - 1), end, count + 1);
        }
        if(location.Item2 < elevations[0].Length - 1 && elevations[location.Item1][location.Item2 + 1] - currentEl >= -1) {
            Process(elevations, best, (location.Item1, location.Item2 + 1), end, count + 1);
        }
    }

    public static (int[][], (int, int), (int, int)) ReadInput(string path) {
        var lines = File.ReadLines("input.txt").ToArray();
        var b = (int)'a';
        var elevations = lines
            .Select(l => l.ToCharArray()
                .Select(c => c == 'S' ? 0 : (c == 'E' ? 25 : (int)c - b)).ToArray())
            .ToArray();
        var (start, end) = GetStartEnd(lines);
        return (elevations, start, end);
    }

    public static ((int, int), (int, int)) GetStartEnd(string[] lines){
        var start = (0, 0);
        var end = (0, 0);
        for(var i = 0; i < lines.Length; i++) {
            for (var j = 0; j < lines[0].Length; j++) {
                if(lines[i][j] == 'S') {
                    start = (i, j);
                } else if (lines[i][j] == 'E') {
                    end = (i, j);
                }
            }
        }
        return (start, end);
    }
}