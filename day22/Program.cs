internal class Program
{
    private enum Tile { Empty, Nothing, Wall }
    private enum Direction { Up, Down, Left, Right }
    private enum Zone {Top, Bottom, Front, Back, Left, Right }

    private const int Side = 4;
    private const string path = "test.txt";

    private static void Main(string[] args)
    {
        var lines = File.ReadLines(path).ToArray();
        var width = lines.Take(lines.Length - 2).Max(l => l.Length);
        var cave = lines.Take(lines.Length - 2)
            .Select(l => {
                var arr = Enumerable.Repeat(Tile.Nothing, width).ToList();
                for(var i = 0; i < l.Length; i++) {
                    arr[i] = l[i] switch {
                        ' ' => Tile.Nothing,
                        '#' => Tile.Wall,
                        '.' => Tile.Empty
                    };
                }
                return arr;
            })
            .ToList();
        var instructions = lines.Last();

        Calculate1(cave, instructions);
    }

    private static void Calculate1(List<List<Tile>> cave, string instructions) {
        var direction = Direction.Right;
        var position = (x: cave[0].IndexOf(Tile.Empty), y: 0 );
        var i = 0;
        var width = cave[0].Count();
        while(i < instructions.Length){
            if(instructions[i] == 'L' || instructions[i] == 'R') {
                direction = Turn(direction, instructions[i]);
                i++;
            } else {
                var number = instructions[i].ToString();
                while(++i < instructions.Length && char.IsDigit(instructions[i])) {
                    number += instructions[i];
                }
                var moveCount = int.Parse(number);
                for(var j = 0; j < moveCount; j++) {
                    switch(direction) {
                        case Direction.Right: {
                            var nextX = position.x == width - 1
                                ? 0
                                : position.x + 1;
                            if(cave[position.y][nextX] == Tile.Nothing) {
                                nextX = cave[position.y].FindIndex(t => t != Tile.Nothing);
                            }
                            if(cave[position.y][nextX] == Tile.Empty) {
                                position.x = nextX;
                            }
                            break;
                        }
                        case Direction.Left: {
                            var nextX = position.x == 0
                                ? width - 1 
                                : position.x - 1;
                            if(cave[position.y][nextX] == Tile.Nothing) {
                                nextX = cave[position.y].FindLastIndex(t => t != Tile.Nothing);
                            }
                            if(cave[position.y][nextX] == Tile.Empty) {
                                position.x = nextX;
                            }
                            break;
                        }
                        case Direction.Up: {
                            var nextY = position.y == 0
                                ? cave.Count - 1 
                                : position.y - 1;
                            if(cave[nextY][position.x] == Tile.Nothing) {
                                nextY = cave
                                    .Select((_, i) => i)
                                    .ToList()
                                    .FindLastIndex(i => cave[i][position.x] != Tile.Nothing);
                            }
                            if(cave[nextY][position.x] == Tile.Empty) {
                                position.y = nextY;
                            }
                            break;
                        }
                        case Direction.Down: {
                            var nextY = position.y == cave.Count - 1 
                                ? 0 
                                : position.y + 1;
                            if(cave[nextY][position.x] == Tile.Nothing) {
                                nextY = cave
                                    .Select((_, i) => i)
                                    .ToList()
                                    .FindIndex(i => cave[i][position.x] != Tile.Nothing);
                            }
                            if(cave[nextY][position.x] == Tile.Empty) {
                                position.y = nextY;
                            }
                            break;
                        }
                    }
                }
            }
            Console.WriteLine($"Pos: {position} Direction: {direction}");
        }

        GetResult(position, direction);
    }

    private static void Calculate2(List<List<Tile>> c, string instructions) {
        var width = c[0].Count();
        var direction = Direction.Right;
        var zone = Zone.Top;
        var zones = GetZones(c, Side);
        var position = (x: zones[zone][0].IndexOf(Tile.Empty), y: 0 );
        var i = 0;
        while(i < instructions.Length){
            if(instructions[i] == 'L' || instructions[i] == 'R') {
                direction = Turn(direction, instructions[i]);
                i++;
            } else {
                var number = instructions[i].ToString();
                while(++i < instructions.Length && char.IsDigit(instructions[i])) {
                    number += instructions[i];
                }
                var moveCount = int.Parse(number);
                for(var j = 0; j < moveCount; j++) {
                    switch(direction) {
                        case Direction.Right: {
                            var nextX = position.x == width - 1
                                ? 0
                                : position.x + 1;
                            if(cave[position.y][nextX] == Tile.Nothing) {
                                nextX = cave[position.y].FindIndex(t => t != Tile.Nothing);
                            }
                            if(cave[position.y][nextX] == Tile.Empty) {
                                position.x = nextX;
                            }
                            break;
                        }
                        case Direction.Left: {
                            var nextX = position.x == 0
                                ? width - 1 
                                : position.x - 1;
                            if(cave[position.y][nextX] == Tile.Nothing) {
                                nextX = cave[position.y].FindLastIndex(t => t != Tile.Nothing);
                            }
                            if(cave[position.y][nextX] == Tile.Empty) {
                                position.x = nextX;
                            }
                            break;
                        }
                        case Direction.Up: {
                            var nextY = position.y == 0
                                ? cave.Count - 1 
                                : position.y - 1;
                            if(cave[nextY][position.x] == Tile.Nothing) {
                                nextY = cave
                                    .Select((_, i) => i)
                                    .ToList()
                                    .FindLastIndex(i => cave[i][position.x] != Tile.Nothing);
                            }
                            if(cave[nextY][position.x] == Tile.Empty) {
                                position.y = nextY;
                            }
                            break;
                        }
                        case Direction.Down: {
                            var nextY = position.y == cave.Count - 1 
                                ? 0 
                                : position.y + 1;
                            if(cave[nextY][position.x] == Tile.Nothing) {
                                nextY = cave
                                    .Select((_, i) => i)
                                    .ToList()
                                    .FindIndex(i => cave[i][position.x] != Tile.Nothing);
                            }
                            if(cave[nextY][position.x] == Tile.Empty) {
                                position.y = nextY;
                            }
                            break;
                        }
                    }
                }
            }
            Console.WriteLine($"Pos: {position} Direction: {direction}");
        }

        GetResult(position, direction);
    }

    private static void GetResult((int x, int y) position, Direction direction) {
        var result = 1000 * (position.y + 1) + 4 * (position.x + 1) 
            + (direction switch {
                Direction.Up => 3,
                Direction.Right => 0,
                Direction.Down => 1,
                Direction.Left => 2,
            });
        Console.WriteLine(result);
    }

    private static Dictionary<Zone, List<List<Tile>>> GetZones(List<List<Tile>> cave) {
        var zones = new Dictionary<Zone, List<List<Tile>>>();
        var topStart = cave[0].FindIndex(t => t != Tile.Nothing);
        zones[Zone.Top] = cave.Take(Side)
            .Select(r => r.Skip(topStart).Take(Side).ToList())
            .ToList();
        zones[Zone.Front] = cave.Skip(Side).Take(Side)
            .Select(r => r.Skip(topStart).Take(Side).ToList())
            .ToList();
        zones[Zone.Bottom] = cave.Skip(Side*2).Take(Side)
            .Select(r => r.Skip(topStart).Take(Side).ToList())
            .ToList();
        var leftStart = cave.Select((_, i) => i)
            .ToList()
            .FindIndex(i => cave[i][topStart - 1] != Tile.Nothing);
        zones[Zone.Left] = cave.Skip(leftStart).Take(Side)
            .Select(r => r.Skip(topStart - Side).Take(Side).ToList())
            .ToList();
        var rightStart = cave.Select((_, i) => i)
            .ToList()
            .FindIndex(i => cave[i][topStart + Side] != Tile.Nothing);
        zones[Zone.Right] = cave.Skip(rightStart).Take(Side)
            .Select(r => r.Skip(topStart + Side).Take(Side).ToList())
            .ToList();
        if(cave.Count / Side == 4) {
            zones[Zone.Back] = cave.Skip(Side * 3).Take(Side)
                .Select(r => r.Take(Side).ToList())
                .ToList();
        } else {
            zones[Zone.Back] = cave.Skip(Side).Take(Side)
                .Select(r => r.Take(Side).ToList())
                .ToList();
        }

        return zones;
    }

    private static Dictionary<(Zone, Direction), (Zone, Direction)> GetConnections() {
        return Side == 4
            ? new Dictionary<(Zone, Direction), (Zone, Direction)> {
                {(Zone.Top, Direction.Down), (Zone.Front, Direction.Down)},
                {(Zone.Top, Direction.Left), (Zone.Left, Direction.Down)},
                {(Zone.Top, Direction.Up), (Zone.Back, Direction.Up)},
                {(Zone.Top, Direction.Right), (Zone.Right, Direction.Right)},
            }
            : null;
    }

    private static Direction Turn(Direction current, char turn) {
        if(turn == 'R') {
            return current switch {
                Direction.Up => Direction.Right,
                Direction.Right => Direction.Down,
                Direction.Down => Direction.Left,
                Direction.Left => Direction.Up
            };
        }       
            return current switch {
                Direction.Up => Direction.Left,
                Direction.Right => Direction.Up,
                Direction.Down => Direction.Right,
                Direction.Left => Direction.Down
            };
    }
}