internal class Program
{
    private static void Main(string[] args)
    {
        var coords = File.ReadLines("input.txt")
            .Select(l => l.Split(',').Select(int.Parse).ToArray())
            .Select(i => (x: i[0], y: i[1], z: i[2]))
            .ToHashSet();
        CountSides(coords);

        FillCenter(coords);
        CountSides(coords);
    }

    private static void FillCenter(HashSet<(int x, int y, int z)> coords) {
        foreach(var coord in coords.ToList()) {
            var adjacents = GetAdjacent(coord).Where(c => !coords.Contains(c));
            foreach(var adjacent in adjacents) {
                var search = new HashSet<(int x, int y, int z)> {  };
                if(IsInterior(adjacent, coords, search)) {
                    coords.UnionWith(search);
                }
            }
        } 
    }

    private static bool IsInterior(
        (int x, int y, int z) current,
        HashSet<(int x, int y, int z)> coords,
        HashSet<(int x, int y, int z)> search)
    {
        if(current.x < 0 || current.x > 20
            || current.y < 0 || current.y > 20
            || current.z < 0 || current.z > 20) {
                return false;
        }
        if(coords.Contains(current) || search.Contains(current)) {
            return true;
        }
        search.Add(current);
        return GetAdjacent(current).All(a => IsInterior(a, coords, search));
    }

    private static void CountSides(HashSet<(int x, int y, int z)> coords) {
        var count = coords.Count * 6;
        foreach(var coord in coords){
            count -= GetAdjacent(coord)
                .Count(c => coords.Contains(c));
        }
        Console.WriteLine(count);
    }

    private static List<(int x, int y, int z)> GetAdjacent((int x, int y, int z) coord) {
        return new List<(int x, int y, int z)> {
            (x: coord.x - 1, y: coord.y, z: coord.z),
            (x: coord.x + 1, y: coord.y, z: coord.z),
            (x: coord.x, y: coord.y - 1, z: coord.z),
            (x: coord.x, y: coord.y + 1, z: coord.z),
            (x: coord.x, y: coord.y, z: coord.z - 1),
            (x: coord.x, y: coord.y, z: coord.z + 1),
        };
    }
}