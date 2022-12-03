internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        var lines = File.ReadLines("input1.txt");
        var scores1 = lines.Select(l => l.Split().Select(c => c[0]).ToList())
            .Select(cs => ResultScore(cs[0], cs[1]) + ChoiceScore(cs[1])).Sum();
        var scores2 = lines.Select(l => l.Split().Select(c => c[0]).ToList())
            .Select(cs => ResultScore2(cs[1]) + ChoiceScore2(cs[0], cs[1])).Sum();
        Console.WriteLine(scores1);
        Console.WriteLine(scores2);
    }
    private static int ResultScore(char c1, char c2) => new List<char> {c1, c2} switch {
            ['A', 'X'] => 3,
            ['A', 'Y'] => 6,
            ['A', 'Z'] => 0,
            ['B', 'X'] => 0,
            ['B', 'Y'] => 3,
            ['B', 'Z'] => 6,
            ['C', 'X'] => 6,
            ['C', 'Y'] => 0,
            ['C', 'Z'] => 3,
    };

    private static int ChoiceScore(char c) => c switch {
        'X' => 1,
        'Y' => 2,
        'Z' => 3,
    };

    private static int ResultScore2(char c) => c switch {
        'X' => 0,
        'Y' => 3,
        'Z' => 6,
    };

    private static int ChoiceScore2(char c1, char c2) => new List<char> {c1, c2} switch {
            ['A', 'X'] => 3,
            ['A', 'Y'] => 1,
            ['A', 'Z'] => 2,
            ['B', 'X'] => 1,
            ['B', 'Y'] => 2,
            ['B', 'Z'] => 3,
            ['C', 'X'] => 2,
            ['C', 'Y'] => 3,
            ['C', 'Z'] => 1,
    };
}