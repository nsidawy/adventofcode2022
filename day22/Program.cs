internal class Program
{
    private enum Tile { Empty, Nothing, Wall }
    private enum Direction { Up, Down, Left, Right }
    private enum Zone {Top, Bottom, Front, Back, Left, Right }

    private const int Side = 50;
    private const string path = "input.txt";

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
        Calculate2(cave, instructions);
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
        var zones = GetZones(c);
        var connections = GetConnections();
        var position = (x: zones[zone][0].IndexOf(Tile.Empty), y: 0 );
        var i = 0;
        while(i < instructions.Length){
            var ins = instructions[i];
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
                    var nextPosition = direction switch {
                        Direction.Up => (x: position.x, y: position.y - 1),
                        Direction.Down => (x: position.x, y: position.y + 1),
                        Direction.Right => (x: position.x + 1, y: position.y),
                        Direction.Left => (x: position.x - 1, y: position.y),
                    };
                    var nextZone = zone;
                    Direction nextDirection = direction;
                    if((nextPosition.x == -1 || nextPosition.x == Side
                        || nextPosition.y == -1 || nextPosition.y == Side)) {
                        (nextZone, nextDirection) = connections[(zone, direction)];
                        switch(nextDirection) {
                            case Direction.Up: {
                                nextPosition.y = Side - 1;
                                nextPosition.x = direction switch {
                                    Direction.Up => position.x,
                                    Direction.Down => Side - 1 -position.x,
                                    Direction.Right => position.y,
                                    Direction.Left => Side - 1 - position.y,
                                };
                                break;
                            }
                            case Direction.Down: {
                                nextPosition.y = 0;
                                nextPosition.x = direction switch {
                                    Direction.Up => Side - 1 - position.x,
                                    Direction.Down => position.x,
                                    Direction.Right => Side - 1 - position.y,
                                    Direction.Left => position.y,
                                };
                                break;
                            }
                            case Direction.Left: {
                                nextPosition.x = Side - 1;
                                nextPosition.y = direction switch {
                                    Direction.Up => Side - 1 - position.x,
                                    Direction.Down => position.x,
                                    Direction.Right => Side - 1 - position.y,
                                    Direction.Left => position.y,
                                };
                                break;
                            }
                            case Direction.Right: {
                                nextPosition.x = 0;
                                nextPosition.y = direction switch {
                                    Direction.Up => position.x,
                                    Direction.Down => Side - 1 - position.x,
                                    Direction.Right => position.y,
                                    Direction.Left => Side - 1 - position.y,
                                };
                                break;
                            }
                        }
                    } 
                    if(zones[nextZone][nextPosition.y][nextPosition.x] == Tile.Empty) {
                        position = nextPosition;
                        zone = nextZone;
                        direction = nextDirection;
                    } else  if(zones[nextZone][nextPosition.y][nextPosition.x] == Tile.Nothing) {
                        throw new InvalidOperationException();
                    }
                }
            }
            Console.WriteLine($"Ins: {ins}; Zone: {zone}; Pos: {position}; Direction: {direction}");
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
                {(Zone.Top, Direction.Up), (Zone.Back, Direction.Down)},
                {(Zone.Top, Direction.Right), (Zone.Right, Direction.Left)},

                {(Zone.Front, Direction.Down), (Zone.Bottom, Direction.Down)},
                {(Zone.Front, Direction.Left), (Zone.Left, Direction.Left)},
                {(Zone.Front, Direction.Up), (Zone.Top, Direction.Up)},
                {(Zone.Front, Direction.Right), (Zone.Right, Direction.Down)},

                {(Zone.Bottom, Direction.Down), (Zone.Back, Direction.Up)},
                {(Zone.Bottom, Direction.Left), (Zone.Left, Direction.Up)},
                {(Zone.Bottom, Direction.Up), (Zone.Front, Direction.Up)},
                {(Zone.Bottom, Direction.Right), (Zone.Right, Direction.Right)},

                {(Zone.Left, Direction.Down), (Zone.Bottom, Direction.Right)},
                {(Zone.Left, Direction.Left), (Zone.Back, Direction.Left)},
                {(Zone.Left, Direction.Up), (Zone.Top, Direction.Right)},
                {(Zone.Left, Direction.Right), (Zone.Front, Direction.Right)},

                {(Zone.Right, Direction.Down), (Zone.Back, Direction.Left)},
                {(Zone.Right, Direction.Left), (Zone.Bottom, Direction.Left)},
                {(Zone.Right, Direction.Up), (Zone.Front, Direction.Left)},
                {(Zone.Right, Direction.Right), (Zone.Top, Direction.Left)},

                {(Zone.Back, Direction.Down), (Zone.Bottom, Direction.Up)},
                {(Zone.Back, Direction.Left), (Zone.Right, Direction.Up)},
                {(Zone.Back, Direction.Up), (Zone.Top, Direction.Down)},
                {(Zone.Back, Direction.Right), (Zone.Left, Direction.Right)},
            }
            : new Dictionary<(Zone, Direction), (Zone, Direction)> {
                {(Zone.Top, Direction.Down), (Zone.Front, Direction.Down)},
                {(Zone.Top, Direction.Left), (Zone.Left, Direction.Right)},
                {(Zone.Top, Direction.Up), (Zone.Back, Direction.Right)},
                {(Zone.Top, Direction.Right), (Zone.Right, Direction.Right)},

                {(Zone.Front, Direction.Down), (Zone.Bottom, Direction.Down)},
                {(Zone.Front, Direction.Left), (Zone.Left, Direction.Down)},
                {(Zone.Front, Direction.Up), (Zone.Top, Direction.Up)},
                {(Zone.Front, Direction.Right), (Zone.Right, Direction.Up)},

                {(Zone.Bottom, Direction.Down), (Zone.Back, Direction.Left)},
                {(Zone.Bottom, Direction.Left), (Zone.Left, Direction.Left)},
                {(Zone.Bottom, Direction.Up), (Zone.Front, Direction.Up)},
                {(Zone.Bottom, Direction.Right), (Zone.Right, Direction.Left)},

                {(Zone.Left, Direction.Down), (Zone.Back, Direction.Down)},
                {(Zone.Left, Direction.Left), (Zone.Top, Direction.Right)},
                {(Zone.Left, Direction.Up), (Zone.Front, Direction.Right)},
                {(Zone.Left, Direction.Right), (Zone.Bottom, Direction.Right)},

                {(Zone.Right, Direction.Down), (Zone.Front, Direction.Left)},
                {(Zone.Right, Direction.Left), (Zone.Top, Direction.Left)},
                {(Zone.Right, Direction.Up), (Zone.Back, Direction.Up)},
                {(Zone.Right, Direction.Right), (Zone.Bottom, Direction.Left)},

                {(Zone.Back, Direction.Down), (Zone.Right, Direction.Down)},
                {(Zone.Back, Direction.Left), (Zone.Top, Direction.Down)},
                {(Zone.Back, Direction.Up), (Zone.Left, Direction.Up)},
                {(Zone.Back, Direction.Right), (Zone.Bottom, Direction.Up)},
            };
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