internal class Program
{
    private static void Main(string[] args)
    {
        var lines = File.ReadLines("input.txt")
            .Select(l => l.Split(" -> ").Select(s => {
                var ints = s.Split(",").Select(int.Parse).ToList();
                return (x: ints[0], y: ints[1]);
            }).ToList())
            .ToList();
        var cave = GetCave(lines, false);
        var part1 = DropSand(cave);
        Print(cave);
        Console.WriteLine(part1);
        cave = GetCave(lines, true);
        var part2 = DropSand(cave);
        Print(cave);
        Console.WriteLine(part2);
    }

    public static int DropSand(bool[][] cave) {
        var count = 0;
        while(true) {
            var position = (x: 500, y: 0);
            if(cave[position.y][position.x]) {
                break;
            }
            var fellOff = false;
            while(true) {
                if(position.y+1 == cave.Length) {
                    fellOff = true;
                    break;
                } else if(!cave[position.y+1][position.x]) {
                    position = (x: position.x, y: position.y + 1);
                } else if (!cave[position.y+1][position.x-1]) {
                    position = (x: position.x-1, y: position.y + 1);
                } else if (!cave[position.y+1][position.x+1]) {
                    position = (x: position.x+1, y: position.y + 1);
                } else {
                    break;
                }
            }
            if(fellOff) {
                break;
            }
            cave[position.y][position.x] = true;
            count++;
        }
        return count;
    }

    public static bool[][] GetCave(List<List<(int x, int y)>> lines, bool hasFloor) {
        var maxX = lines.Max(l => l.Max(c => c.x));
        var maxY = lines.Max(l => l.Max(c => c.y));
        var cave = Enumerable.Range(0, maxY + 2 + (hasFloor ? 1 : 0))
            .Select(_ => Enumerable.Repeat(false, Math.Max(maxX, 500) + (hasFloor ? 1000 : 2))
                .ToArray())
            .ToArray();
        lines.ForEach(line => {
            for(var i = 0; i < line.Count - 1; i++) {
                if(line[i].x == line[i+1].x) {
                    for(var j = Math.Min(line[i].y, line[i+1].y); j <= Math.Max(line[i].y, line[i+1].y); j++) {
                        cave[j][line[i].x] = true;
                    }
                } else {
                    for(var j = Math.Min(line[i].x, line[i+1].x); j <= Math.Max(line[i].x, line[i+1].x); j++) {
                        cave[line[i].y][j] = true;
                    }
                }
            }
        });
        if(hasFloor) {
            for(var x = 0; x < cave[0].Length; x++) {
                cave[cave.Length - 1][x] = true;
            }
        }
        return cave;
    }

    public static void Print(bool[][] cave) {
        var minFilledX = cave
            .Take(cave.Length - 1)
            .Select(y => 
                y.Select((b, i) => b ? i : int.MaxValue)
                .Min())
            .Min() - 1;
        var maxFilledX = cave
            .Take(cave.Length - 1)
            .Select(y => 
                y.Select((b, i) => b ? i : int.MinValue)
                .Max())
            .Max();
        for(var y = 0; y < cave.Length; y++) {
            for(var x = minFilledX; x <= maxFilledX; x++) {
                if(cave[y][x]) {
                    Console.Write("#");
                } else {
                    Console.Write(".");
                }
            }
            Console.WriteLine();
        }
    }
}