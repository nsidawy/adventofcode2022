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
    private static int ResultScore(char c1, char c2) => c1 switch {
        'A' => c2 switch {
            'X' => 3,
            'Y' => 6,
            'Z' => 0,
        },
        'B' => c2 switch {
            'X' => 0,
            'Y' => 3,
            'Z' => 6,
        },
        'C' => c2 switch {
            'X' => 6,
            'Y' => 0,
            'Z' => 3,
        },
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

    private static int ChoiceScore2(char c1, char c2) => c1 switch {
        'A' => c2 switch {
            'X' => 3,
            'Y' => 1,
            'Z' => 2,
        },
        'B' => c2 switch {
            'X' => 1,
            'Y' => 2,
            'Z' => 3,
        },
        'C' => c2 switch {
            'X' => 2,
            'Y' => 3,
            'Z' => 1,
        },
    };
}