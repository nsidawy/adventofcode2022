internal class Program
{
    private static void Main(string[] args)
    {
        var sum = File.ReadLines("input.txt")
            .Select(ParseSnafu).Sum();
        Console.WriteLine(sum);
        var max = 15;
        while(true) {
            Console.WriteLine(Math.Pow(5, max -1));
            var snafu = Generate(max - 1, "2", sum);
            if(snafu != null) {
                Console.WriteLine(snafu);
                break;
            }
            snafu = Generate(max - 1, "1", sum);
            if(snafu != null) {
                Console.WriteLine(snafu);
                break;
            }
            max++;
        }
    }

    public static string? Generate(int digits, string snafu, long target) {
        if(digits == 0) {
            return ParseSnafu(snafu) == target ? snafu : null;
        }

        return Generate(digits - 1, snafu + '0', target)
            ?? Generate(digits - 1, snafu + '1', target)
            ?? Generate(digits - 1, snafu + '2', target)
            ?? Generate(digits - 1, snafu + '-', target)
            ?? Generate(digits - 1, snafu + '=', target);
    }

    private static long ParseSnafu(string s) {
         var revl = s.Reverse().ToList();
         var total = 0;
         for(var i = 0; i < revl.Count; i++) {
             var b = (int)Math.Pow(5, i);
             total += b * (revl[i] switch {
                 '0' => 0,
                 '1' => 1,
                 '2' => 2,
                 '-' => -1,
                 '=' => -2
             });
         }
         return total;
    }
}