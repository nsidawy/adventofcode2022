internal class Program
{
    private enum Direction {Up, Down, Left, Right}
    private static void Main(string[] args)
    {
        var lines = File.ReadLines("input.txt").ToList();
        var height = lines.Count;
        var width = lines[0].Length;
        var start = (lines[0].IndexOf("."), 0);
        var end = (lines[height - 1].IndexOf("."), height - 1);
        var (blizzards, walls) = GetBlizzards(lines);

        var minute = 1;
        var current = new HashSet<(int, int)> {start};
        while(true) {
            var next = new HashSet<(int, int)>();
            blizzards = blizzards.Select(b => GetNextBlizzard(b, height, width)).ToList();
            var blockers = walls.Union(blizzards.Select(b => (b.Item2, b.Item3))).ToHashSet();
            foreach (var c in current) {
                if(!blockers.Contains(c)) {
                    next.Add(c);
                }
                if(!blockers.Contains((c.Item1 - 1, c.Item2))) {
                    next.Add((c.Item1 - 1, c.Item2));
                }
                if(!blockers.Contains((c.Item1 + 1, c.Item2))) {
                    next.Add((c.Item1 + 1, c.Item2));
                }
                if(c.Item2 != 0 && !blockers.Contains((c.Item1, c.Item2 - 1))) {
                    next.Add((c.Item1, c.Item2 - 1));
                } 
                if(!blockers.Contains((c.Item1, c.Item2 + 1))) {
                    next.Add((c.Item1, c.Item2 + 1));
                }
            }

            if(next.Contains(end)) {
                break;
            }
            current = next;

            minute++;
        }
        Console.WriteLine(minute);

        (blizzards, _) = GetBlizzards(lines);
        minute = 1;
        var current2 = new HashSet<(int, int, bool, bool)> {(start.Item1, start.Item2, false, false)};
        while(true) {
            var next = new HashSet<(int, int, bool, bool)>();
            blizzards = blizzards.Select(b => GetNextBlizzard(b, height, width)).ToList();
            var blockers = walls.Union(blizzards.Select(b => (b.Item2, b.Item3))).ToHashSet();
            foreach (var c in current2) {
                if(!blockers.Contains((c.Item1, c.Item2))) {
                    next.Add(c);
                }
                if(!blockers.Contains((c.Item1 - 1, c.Item2))) {
                    next.Add((c.Item1 - 1, c.Item2,
                        (c.Item1 - 1, c.Item2) == end ? true : c.Item3,
                        c.Item3 && (c.Item1 - 1, c.Item2) == start ? true : c.Item4));
                }
                if(!blockers.Contains((c.Item1 + 1, c.Item2))) {
                    next.Add((c.Item1 + 1, c.Item2,
                        (c.Item1 + 1, c.Item2) == end ? true : c.Item3,
                        c.Item3 && (c.Item1 + 1, c.Item2) == start ? true : c.Item4));
                }
                if(c.Item2 != 0 && !blockers.Contains((c.Item1, c.Item2 - 1))) {
                    next.Add((c.Item1, c.Item2 - 1,
                        (c.Item1, c.Item2 - 1) == end ? true : c.Item3,
                        c.Item3 && (c.Item1, c.Item2 - 1) == start ? true : c.Item4));
                } 
                if(c.Item2 != height -1 && !blockers.Contains((c.Item1, c.Item2 + 1))) {
                    next.Add((c.Item1, c.Item2 + 1,
                        (c.Item1, c.Item2 + 1) == end ? true : c.Item3,
                        c.Item3 && (c.Item1, c.Item2 + 1) == start ? true : c.Item4));
                }
            }

            if(next.Contains((end.Item1, end.Item2, true, true))) {
                break;
            }
            current2 = next;

            minute++;
        }
        Console.WriteLine(minute);
    }

    private static (Direction dir, int x, int y) GetNextBlizzard((Direction dir, int x, int y) blizzard, int height, int width) {
        switch(blizzard.dir) {
            case Direction.Up:
                blizzard.y = blizzard.y == 1 ? height - 2 : blizzard.y - 1;
                return blizzard;
            case Direction.Down:
                blizzard.y = blizzard.y == height - 2 ? 1 : blizzard.y + 1;
                return blizzard;
            case Direction.Left:
                blizzard.x = blizzard.x == 1 ? width - 2 : blizzard.x - 1;
                return blizzard;
            case Direction.Right:
                blizzard.x = blizzard.x == width - 2 ? 1 : blizzard.x + 1;
                return blizzard;
            default:
                throw new InvalidOperationException();
        }
    }

    private static (List<(Direction, int, int)>, HashSet<(int, int)>) GetBlizzards(List<string> lines){
        var blizzards = new List<(Direction, int, int)>();
        var walls = new HashSet<(int, int)>();
        for(var y = 0; y < lines.Count; y++) {
            for(var x = 0; x < lines[0].Length; x++) {
                if(lines[y][x] == '#') {
                    walls.Add((x, y));
                } else if(lines [y][x] != '.') {
                    blizzards.Add((
                      lines[y][x] switch {
                        '>' => Direction.Right,
                        '<' => Direction.Left,
                        '^' => Direction.Up,
                        'v' => Direction.Down,
                      }, x, y));
                }
            }
        }
        return (blizzards, walls);
    }
}