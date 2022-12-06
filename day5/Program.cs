using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        var lines = File.ReadLines("input1.txt");
        var placementLines = lines.TakeWhile(l => l != "").ToArray();
        var ids = Regex.Matches(placementLines[placementLines.Length - 1], @"\d")
            .Select(m => int.Parse(m.Value))
            .ToArray();;

        var stacks = GetStacks(placementLines, ids);
        var instructions = lines.SkipWhile(l => l != "").Skip(1)
            .Select(i => {
                var values = Regex.Matches(i, @"\d+")
                    .Select(m => int.Parse(m.Value))
                    .ToArray();;
                return (values[0], values[1], values[2]);
            }).ToArray();
        // calculate part 1
        foreach (var instruction in instructions){
            var (count, from, to) = instruction;
            for(var i = 0; i < count; i++) {
                if(!stacks[from].Any()) {
                    continue;
                }
                var value = stacks[from].Last();
                stacks[from].RemoveAt(stacks[from].Count - 1);
                stacks[to].Add(value);
            }
        }
        PrintResult(stacks, ids);

        // calculate part 2
        stacks = GetStacks(placementLines, ids);
        foreach (var instruction in instructions){
            var (count, from, to) = instruction;
            var value = stacks[from].TakeLast(count).ToList();
            stacks[from].RemoveRange(Math.Max(0, stacks[from].Count - count), Math.Min(count, stacks[from].Count));
            stacks[to].AddRange(value);
        }

        PrintResult(stacks, ids);
    }

    public static void PrintResult(Dictionary<int, List<char>> stacks, int[] ids) {
        foreach(var id in ids) {
            Console.WriteLine(string.Join(' ', stacks[id]));
        }
        Console.WriteLine($"Answer: {string.Join(' ', ids.Select(r => stacks[r].Last()).ToList())}");
    }

    public static Dictionary<int, List<char>> GetStacks(string[] placementLines, int[] ids) {
        var stacks = new Dictionary<int, List<char>>();
        foreach(var id in ids) {
            stacks[id] = new List<char>();
        }

        foreach (var line in placementLines.Take(placementLines.Length - 1)){
            var index = 1;
            foreach(var result in ids) {
                if(line[index] != ' ') {
                    stacks[(int)result].Insert(0, line[index]);
                }
                index +=4;
            }
        }

        return stacks;
    } 
}
