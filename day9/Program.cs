internal class Program
{
    private static void Main(string[] args)
    {
        var moves = File.ReadLines("input1.txt").ToList();
        Console.WriteLine(GetTailCount(moves, 2));
        Console.WriteLine(GetTailCount(moves, 10));
    }

    public static int GetTailCount(List<string> moves, int length) {
        var positions = Enumerable.Range(0, length).Select(x => new Position()).ToList();
        var visitedY = new HashSet<Position>();
        foreach (var move in moves) {
            //Console.WriteLine(move);
            var m = move.Split(" ");
            for(var i = 0; i < int.Parse(m[1]); i++) {
                var direction = m[0];
                Action<Position> nextPosition = direction switch {
                    "D" => p => p.Change(0, -1),
                    "U" => p => p.Change(0, 1),
                    "L" => p => p.Change(-1, 0),
                    "R" => p => p.Change(1, 0),
                };
                nextPosition(positions[0]);
                var j = 1;
                while(j < positions.Count){
                    var targetPosition = positions[j - 1];
                    Func<Position, Position, bool> segmentCompare;
                    if (positions[j].Y + 2 == targetPosition.Y) {
                        if(positions[j].X == targetPosition.X){
                            nextPosition = p => p.Change(0, 1);
                            segmentCompare = (t, c) => t.X == c.X && t.Y == c.Y + 1;
                        } else if(positions[j].X + 1 == targetPosition.X){
                            nextPosition = p => p.Change(1, 1);
                            segmentCompare = (t, c) => (t.X == c.X + 1 && t.Y == c.Y)
                                || (t.X == c.X && t.Y == c.Y + 1)
                                || (t.X == c.X + 1 && t.Y == c.Y + 1); 
                        } else if(positions[j].X - 1 == targetPosition.X){
                            nextPosition = p => p.Change(-1, 1);
                            segmentCompare = (t, c) => (t.X == c.X - 1 && t.Y == c.Y)
                                || (t.X == c.X && t.Y == c.Y + 1)
                                || (t.X == c.X - 1 && t.Y == c.Y + 1); 
                        } else {
                            break;
                        }
                    } else if (positions[j].Y - 2 == targetPosition.Y) {
                        if(positions[j].X == targetPosition.X){
                            nextPosition = p => p.Change(0, -1);
                            segmentCompare = (t, c) => t.X == c.X && t.Y == c.Y - 1;
                        } else if(positions[j].X + 1 == targetPosition.X){
                            nextPosition = p => p.Change(1, -1);
                            segmentCompare = (t, c) => (t.X == c.X + 1 && t.Y == c.Y)
                                || (t.X == c.X && t.Y == c.Y - 1)
                                || (t.X == c.X + 1 && t.Y == c.Y - 1); 
                        } else if(positions[j].X - 1 == targetPosition.X){
                            nextPosition = p => p.Change(-1, -1);
                            segmentCompare = (t, c) => (t.X == c.X - 1 && t.Y == c.Y)
                                || (t.X == c.X && t.Y == c.Y - 1)
                                || (t.X == c.X - 1 && t.Y == c.Y - 1); 
                        } else {
                            break;
                        }
                    } else if (positions[j].X + 2 == targetPosition.X) {
                        if(positions[j].Y == targetPosition.Y){
                            nextPosition = p => p.Change(1, 0);
                            segmentCompare = (t, c) => t.X == c.X + 1 && t.Y == c.Y;
                        } else if(positions[j].Y + 1 == targetPosition.Y){
                            nextPosition = p => p.Change(1, 1);
                            segmentCompare = (t, c) => (t.X == c.X + 1 && t.Y == c.Y)
                                || (t.X == c.X && t.Y == c.Y + 1)
                                || (t.X == c.X + 1 && t.Y == c.Y + 1); 
                        } else if(positions[j].Y - 1 == targetPosition.Y){
                            nextPosition = p => p.Change(1, -1);
                            segmentCompare = (t, c) => (t.X == c.X + 1 && t.Y == c.Y)
                                || (t.X == c.X && t.Y == c.Y - 1)
                                || (t.X == c.X + 1 && t.Y == c.Y - 1); 
                        } else {
                            break;
                        }
                    } else if (positions[j].X - 2 == targetPosition.X) {
                        if(positions[j].Y == targetPosition.Y){
                            nextPosition = p => p.Change(-1, 0);
                            segmentCompare = (t, c) => t.X == c.X - 1 && t.Y == c.Y;
                        } else if(positions[j].Y + 1 == targetPosition.Y){
                            nextPosition = p => p.Change(-1, 1);
                            segmentCompare = (t, c) => (t.X == c.X - 1 && t.Y == c.Y)
                                || (t.X == c.X && t.Y == c.Y + 1)
                                || (t.X == c.X - 1 && t.Y == c.Y + 1); 
                        } else if(positions[j].Y - 1 == targetPosition.Y){
                            nextPosition = p => p.Change(-1, -1);
                            segmentCompare = (t, c) => (t.X == c.X - 1 && t.Y == c.Y)
                                || (t.X == c.X && t.Y == c.Y - 1)
                                || (t.X == c.X - 1 && t.Y == c.Y - 1); 
                        } else {
                            break;
                        }
                    } else {
                        break;
                    }
                    do {
                        targetPosition = positions[j].Clone();
                        nextPosition(positions[j]);
                        j++;
                    } while(j < positions.Count && segmentCompare(targetPosition, positions[j]));
                }
                //Console.WriteLine(string.Join(',', positions.Select((p,i) => $"{i}:{p}").ToList()));
                //PrintPositions(positions);
                visitedY.Add(positions.Last());
            }
        }
        return visitedY.Count;
    }

    public static void PrintPositions(List<Position> positions) {
        var minX = positions.Select(p => p.X).Min();
        var maxX = positions.Select(p => p.X).Max();
        var minY = positions.Select(p => p.Y).Min();
        var maxY = positions.Select(p => p.Y).Max();
        for(var y = maxY; y >= minY; y--) {
            for(var x = minX; x <= maxX; x++){
                var i = positions.FindIndex(p => p.X == x && p.Y == y);
                Console.Write(i == -1 ? "." : i);
            }
            Console.WriteLine();
        }
    }

    public class Position {
        public Position()
        {
            this.X = 0;
            this.Y = 0;
        }
        public Position(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public int X {get;set;}
        public int Y {get;set;}
        public void Change(int dx, int dy) {
            this.X += dx;
            this.Y += dy;
        }
        public override string ToString() => $"({this.X},{this.Y})";
        public override bool Equals(object? obj) => obj is Position && ((Position)obj).X == this.X && ((Position)obj).Y == this.Y;
        public override int GetHashCode() => this.X + this.Y * 1000;
        public Position Clone() => new Position(this.X, this.Y);
    }
}
