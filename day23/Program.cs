internal class Program
{
    private static List<List<Func<(int, int), (int, int)>>> change =
        new List<List<Func<(int, int), (int, int)>>> {
            new List<Func<(int, int), (int, int)>> {
                (c) => (c.Item1, c.Item2 - 1),
                (c) => (c.Item1 - 1, c.Item2 - 1),
                (c) => (c.Item1 + 1, c.Item2 - 1),
            },
            new List<Func<(int, int), (int, int)>> {
                (c) => (c.Item1, c.Item2 + 1),
                (c) => (c.Item1 - 1, c.Item2 + 1),
                (c) => (c.Item1 + 1, c.Item2 + 1),
            },
            new List<Func<(int, int), (int, int)>> {
                (c) => (c.Item1 - 1, c.Item2),
                (c) => (c.Item1 - 1, c.Item2 + 1),
                (c) => (c.Item1 - 1, c.Item2 - 1),
            },
            new List<Func<(int, int), (int, int)>> {
                (c) => (c.Item1 + 1, c.Item2),
                (c) => (c.Item1 + 1, c.Item2 + 1),
                (c) => (c.Item1 + 1, c.Item2 - 1),
            },
        };

    private static void Main(string[] args)
    {
        var rounds = 10;
        var path = "input.txt";

        var elves = GetElves(path);
        for(var i = 0; i < rounds; i++) {
            elves = GetNext(elves, i);
        }

        var minX = elves.Min(e => e.Item1);
        var maxX = elves.Max(e => e.Item1);
        var minY = elves.Min(e => e.Item2);
        var maxY = elves.Max(e => e.Item2);
        var result = (maxX - minX + 1) * (maxY - minY + 1) - elves.Count;
        Console.WriteLine(result);

        elves = GetElves(path);
        var round = 0;
        while(true) {
            var next = GetNext(elves, round);
            if(next.All(c => elves.Contains(c))) {
                break;
            } 
            elves = next;
            round++;
        }
        Console.WriteLine(round + 1);
    }

    public static HashSet<(int, int)> GetElves(string path) {
        var lines = File.ReadLines(path).ToList();
        var elves = new HashSet<(int, int)>();
        for(var y = 0; y < lines.Count; y++) {
            for(var x = 0; x < lines[0].Length; x++) {
                if(lines[y][x] == '#') {
                    elves.Add((x, y));
                }
            }
        }
        return elves;
    }

    public static HashSet<(int, int)> GetNext(HashSet<(int, int)> elves, int round) {
        var proposed = new Dictionary<(int, int), (int, int)>();
        var counts = new Dictionary<(int, int), int>();
        foreach(var elf in elves) {
            var target = elf;
            if (CanMove(elf, elves)) {
                for(var j = 0; j < change.Count; j++) {
                    var changeIndex = (j + round) % 4;
                    if(change[changeIndex].All(c => !elves.Contains(c(elf)))) {
                        target = change[changeIndex][0](elf);
                        break;
                    }
                }
            }            

            proposed[elf] = target;
            if(counts.ContainsKey(target)) {
                counts[target] = counts[target] + 1;
            } else {
                counts[target] = 1;
            }
        }

        var newElves = new HashSet<(int, int)>();
        foreach(var elf in elves) {
            if(counts[proposed[elf]] > 1) {
                newElves.Add(elf);
            } else {
                newElves.Add(proposed[elf]);
            }
        }
        return newElves;
    }

    private static bool CanMove((int, int) check, HashSet<(int, int)> elves) {
        return elves.Contains((check.Item1, check.Item2 + 1))
            || elves.Contains((check.Item1 + 1, check.Item2 + 1))
            || elves.Contains((check.Item1 - 1, check.Item2 + 1))

            || elves.Contains((check.Item1, check.Item2 - 1))
            || elves.Contains((check.Item1 + 1, check.Item2 - 1))
            || elves.Contains((check.Item1 - 1, check.Item2 - 1))

            || elves.Contains((check.Item1 + 1, check.Item2))
            || elves.Contains((check.Item1 - 1, check.Item2));
    }
}