internal class Program
{
    public static List<List<(int x, int y)>> rocks = new List<List<(int x, int y)>>{
        new List<(int x, int y)> {
            (x: 0, y: 0), (x: 1, y: 0), (x: 2, y: 0), (x: 3, y: 0)
        },
        new List<(int x, int y)> {
            (x: 1, y: 0), (x: 1, y: 1), (x: 0, y: 1), (x: 2, y: 1), (x: 1, y: 2)
        },
        new List<(int x, int y)> {
            (x: 0, y: 0), (x: 1, y: 0), (x: 2, y: 0), (x: 2, y: 1), (x: 2, y: 2)
        },
        new List<(int x, int y)> {
            (x: 0, y: 0), (x: 0, y: 1), (x: 0, y: 2), (x: 0, y: 3)
        },
        new List<(int x, int y)> {
            (x: 0, y: 0), (x: 1, y: 0), (x: 1, y: 1), (x: 0, y: 1)
        },
    };

    private static void Main(string[] args)
    {
        var jets = File.ReadAllLines("test.txt").Single().ToCharArray();
        Simulate(jets, 2022);
        Simulate2(jets, 2022);
        Simulate2(jets, 1000000000000);
    }

    private static void Simulate(char[] jets, long totalRocks) {
        var width = 7;
        var cave = new bool[width, totalRocks * 4];
        var maxY = 0;
        var time = 0;
        Func<(int x, int y), char, (int x, int y)> jetMove = (c, j) => (x: j == '<' ? c.x - 1 : c.x + 1, y: c.y);
        Func<(int x, int y), (int x, int y)> downMove = c => (x: c.x, y: c.y - 1);

        for(var i = 0; i < totalRocks; i++) {
            var x = 2;
            var y = maxY + 4;
            var rock = rocks[i % rocks.Count].Select(c => (x: c.x + x, y: c.y + y)).ToList();
            while(true){
                var jet = jets[time % jets.Length];
                time++;
                var newRock = rock.Select(c => jetMove(c, jet)).ToList();
                if (newRock.All(c => c.x >= 0 && c.x < width && !cave[c.x, c.y])) {
                    rock = newRock;
                }

                newRock = rock.Select(c => downMove(c)).ToList();
                if(newRock.Any(c => c.y < 1 || cave[c.x, c.y])) {
                    break; 
                }
                rock = newRock;
            }
            rock.ForEach(c => cave[c.x, c.y] = true);
            maxY = Math.Max(rock.Max(c => c.y), maxY);
        }
        Console.WriteLine(maxY);
    }
    private static void Simulate2(char[] jets, long totalRocks) {
        var width = 7;
        var cave = new bool[width, 10000000];
        var maxY = (long)-1;
        var time = 0;
        Func<(int x, int y), char, (int x, int y)> jetMove = (c, j) => (x: j == '<' ? c.x - 1 : c.x + 1, y: c.y);
        Func<(int x, int y), (int x, int y)> downMove = c => (x: c.x, y: c.y - 1);
        var memoize = new Dictionary<(bool, bool, bool, bool, bool, bool, bool, int, int), (int, long)>();
        var stepDict = new Dictionary<int, long>();
        (bool, bool, bool, bool, bool, bool, bool, int, int) keySave;
        var i = 1;
        var targetStreak = 20;
        var streak = 0;
        while(true) {
            var x = 2;
            var y = (int)maxY + 4;
            var rock = rocks[(i-1) % rocks.Count].Select(c => (x: c.x + x, y: c.y + y)).ToList();
            while(true){
                var jet = jets[time % jets.Length];
                time++;
                var newRock = rock.Select(c => jetMove(c, jet)).ToList();
                if (newRock.All(c => c.x >= 0 && c.x < width && !cave[c.x, c.y])) {
                    rock = newRock;
                }

                newRock = rock.Select(c => downMove(c)).ToList();
                if(newRock.Any(c => c.y < 0 || cave[c.x, c.y])) {
                    break; 
                }
                rock = newRock;
            }
            rock.ForEach(c => cave[c.x, c.y] = true);
            maxY = Math.Max(rock.Max(c => c.y), maxY);
            var key = (cave[0,maxY],cave[1,maxY],cave[2,maxY],cave[3,maxY],cave[4,maxY],cave[5,maxY],cave[6,maxY], time % jets.Length, i % rocks.Count);
            if(memoize.ContainsKey(key)) {
                streak++;
                if(streak == targetStreak) {
                    keySave = key;
                    break;
                }
            } else {
                streak = 0;
                memoize[key] = (i, maxY);
            }
            stepDict[i] = maxY;
            i++;
        }
        var (saveI, saveYMax) = memoize[keySave];
        var iDiff = i - saveI;
        var startYMax = stepDict[saveI];
        var maxYDiff = maxY - startYMax;
        var iRemaining = (int)((totalRocks - (saveI)) % iDiff);
        var iDiffYMax = (totalRocks - (saveI)) / iDiff * maxYDiff;
        var z = startYMax + iDiffYMax + (stepDict[iRemaining + saveI] - stepDict[saveI]);
        Console.WriteLine(z+1);
    }

    private static void Print(bool[,] cave, int maxY) {
        for(int y = maxY; y >= 0; y--) {
            Console.Write($"{y}: ");
            for(int x = 0; x < 7; x++) {
                Console.Write(cave[x,y] ? "#" : ".");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}