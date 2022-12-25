internal class Program
{
    private static void Main(string[] args)
    {
        var sum = File.ReadLines("input.txt")
            .Select(ParseSnafu).Sum();
        Console.WriteLine("Sum:" + sum);
        var max = 20;
        for(var i = 0; i <= max; i++) {
            sum += (long)(Math.Pow(5, i) * 2);
        }
        var result = string.Empty;
        for(var i = max; i >= 0; i--) {
            var val = (int)(sum / Math.Pow(5, i)); 
            sum %= (long)Math.Pow(5, i);
            result += val switch {
                0 => '=',
                1 => '-',
                2 => '0',
                3 => '1',
                4 => '2',
            };
        }
        Console.WriteLine("Result (SNAFU): " + result);
        Console.WriteLine("Result (decimal): " + ParseSnafu(result));
    }
    
    private static long ParseSnafu(string s) {
         long total = 0;
         for(var i = 0; i < s.Length; i++) {
             var b = (long)Math.Pow(5, i);
             total += b * (s[s.Length - i - 1] switch {
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