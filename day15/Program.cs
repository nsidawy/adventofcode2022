using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        //var targetY = 10;
        //var path = "test.txt";
        //var min = 0;
        //var max = 20;
        var targetY = 2000000;
        var path = "input.txt";
        var min = 0;
        var max = 4000000;
        var coords = File.ReadLines(path)
            .Select(l => {
                var digits = Regex.Matches(l, @"-?\d+")
                    .Select(x => int.Parse(x.Value))
                    .ToArray();
                return (sensor: (x: digits[0], y: digits[1])
                    , beacon: (x: digits[2], y: digits[3]));
            })
            .ToArray();
        Console.WriteLine(GetOccupiedAtY(coords, targetY));
        FindDistressBeacon(coords, min, max);
    }

    public static void FindDistressBeacon(
        ((int x, int y) sensor, (int x, int y) beacon)[] coords,
        int min,
        int max) {
        var occupied = new Dictionary<int, List<(int min, int max)>>();
        foreach(var coord in coords) {
            var dist = GetManhattenDistance(coord.sensor, coord.beacon);
            for(var y = Math.Max((dist * -1) + coord.sensor.y, min); y <= Math.Min(dist + coord.sensor.y, max); y++) {
                if(!occupied.ContainsKey(y)){ 
                    occupied[y] = new List<(int min, int max)>();
                }
                var xRange = dist - Math.Abs(coord.sensor.y - y);
                var xmin = coord.sensor.x - xRange;
                var xmax = coord.sensor.x + xRange;
                if(xmin > max || xmax < min) {
                    continue;
                }
                var range = (min: Math.Max(xmin, min), man: Math.Min(coord.sensor.x + xRange, max));
                occupied[y].Add(range);
            }
        };
        foreach(var key in occupied.Keys) {
            var ranges = occupied[key].OrderBy(x => x.min).ToList();
            var currentRange = ranges[0];
            for(var i = 1; i < ranges.Count; i++) {
                if(!(currentRange.max + 1 >= ranges[i].min)) {
                    Console.WriteLine(((currentRange.max + 1), key));
                    Console.WriteLine(((currentRange.max + 1) * (long)4000000) + key);
                }
                currentRange = (min: currentRange.min, Math.Max(ranges[i].max, currentRange.max));
            }
        }
    }

    public static int GetOccupiedAtY(((int x, int y) sensor, (int x, int y) beacon)[] coords, int targetY) {
        var occupied = new HashSet<(int x, int y)>();
        foreach(var coord in coords) {
            occupied.Add(coord.sensor);
            var manDist = GetManhattenDistance(coord.sensor, coord.beacon);
            var targetYDist = Math.Abs(coord.sensor.y - targetY);
            if(targetYDist > manDist) {
                continue;
            }
            var xRange = manDist - targetYDist;
            for(var x = xRange * -1; x <= xRange; x++) {
                var checkCoord = (x: coord.sensor.x + x, y: targetY);
                if(!coords.Any(c => c.beacon == checkCoord)){
                    occupied.Add(checkCoord);
                }
            }
        }
        return occupied.Count(c => c.y == targetY);
    }

    public static int GetManhattenDistance((int x, int y) sensor, (int x, int y) beacon) {
        return Math.Abs(sensor.x - beacon.x) + Math.Abs(beacon.y - sensor.y);
    }
}