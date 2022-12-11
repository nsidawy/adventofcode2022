internal class Program
{
    private static void Main(string[] args)
    {
        var lines = File.ReadLines("input.txt");
        var cycle = 1;
        var signalStrengths = new List<int>();
        var x = 1;
        Action processCycle = () => {
            Draw(cycle, x);
            cycle++;
            if(cycle == 20 || (cycle - 20) % 40 == 0) {
                signalStrengths.Add(cycle * x);
            }
        };
        foreach (var line in lines) {
            processCycle();
            if(line != "noop") {
                var value = int.Parse(line.Split(" ")[1]);
                processCycle();
                x += value;
            }
        }
        Console.WriteLine();
        Console.WriteLine(string.Join(',', signalStrengths));
        Console.WriteLine(signalStrengths.Sum());
    }

    private static void Draw(int cycle, int x) {
        var rowPosition = (cycle - 1) % 40;
        if(rowPosition == 0) {
            Console.WriteLine();
        }
        Console.Write(Math.Abs(rowPosition - x) <= 1 ? "#" : ".");
    }
}
