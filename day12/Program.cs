internal class Program
{
    private static void Main(string[] args)
    {
        var (elevations, start, end) = ReadInput("input.txt");
        var best = InitializeBest(elevations);
        Process(elevations, best, start, end, 0);
        Console.WriteLine(best[end.Item1][end.Item2]);
        var allStartsScore = GetAllStarts(elevations).Select(s => {
            var best = InitializeBest(elevations);
            Process(elevations, best, s, end, 0);
            return best[end.Item1][end.Item2];
        }).ToList();
        Console.WriteLine(allStartsScore.Min());
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

    public static int[][] InitializeBest(int[][] elevations) {
        return Enumerable.Range(0, elevations.Length)
            .Select(_ => Enumerable.Repeat(int.MaxValue, elevations[0].Length).ToArray())
            .ToArray();
    }

    public static void Process(int[][] elevations, int[][] best, (int, int) location, (int, int) end, int count){
        if(count >= best[location.Item1][location.Item2]) {
            return;
        } 
        best[location.Item1][location.Item2] = count;
        //if(location == end) {
        //    return;
        //}
        var currentEl = elevations[location.Item1][location.Item2];
        if(location.Item1 > 0 && elevations[location.Item1 - 1][location.Item2] - currentEl <= 1) {
            Process(elevations, best, (location.Item1 - 1, location.Item2), end, count + 1);
        }
        if(location.Item1 < elevations.Length - 1 && elevations[location.Item1 + 1][location.Item2] - currentEl <= 1) {
            Process(elevations, best, (location.Item1 + 1, location.Item2), end, count + 1);
        }
        if(location.Item2 > 0 && elevations[location.Item1][location.Item2 - 1] - currentEl <= 1) {
            Process(elevations, best, (location.Item1, location.Item2 - 1), end, count + 1);
        }
        if(location.Item2 < elevations[0].Length - 1 && elevations[location.Item1][location.Item2 + 1] - currentEl <= 1) {
            Process(elevations, best, (location.Item1, location.Item2 + 1), end, count + 1);
        }
    }

    public static IEnumerable<(int, int)> GetAllStarts(int[][] elevations) {
        for(var i = 0; i < elevations.Length; i++) {
            for (var j = 0; j < elevations[0].Length; j++) {
                if(elevations[i][j] == 0){
                    yield return (i, j);
                }
            }
        }
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