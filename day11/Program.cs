using System.Text.RegularExpressions;
using System.Numerics;

internal class Program
{
    private static void Main(string[] args)
    {
        var file = "input.txt";
        var monkeys = GetMonkeys(file);
        Process(monkeys, 20, true);
        var maxes = monkeys.Select(m => m.InspectionCount).Order().TakeLast(2).ToList();
        Console.WriteLine(maxes[0] * maxes[1]);

        monkeys = GetMonkeys(file);
        Process(monkeys, 10000, false);
        maxes = monkeys.Select(m => m.InspectionCount).Order().TakeLast(2).ToList();
        Console.WriteLine(maxes[0] * maxes[1]);
    }

    public static void Process(List<Monkey> monkeys, int rounds, bool useDivsor) {
        for (var i = 0; i < rounds; i++) {
            foreach(var monkey in monkeys) {
                foreach (var item in monkey.Items){
                    monkey.InspectionCount++;
                    var val1 = monkey.Operation[0] == "old"
                        ? item
                        : BigInteger.Parse(monkey.Operation[0]);
                    var val2 = monkey.Operation[2] == "old"
                        ? item
                        : BigInteger.Parse(monkey.Operation[2]);
                    var newVal = monkey.Operation[1] switch {
                        "+" => val1 + val2,
                        "*" => val1 * val2
                    };

                    if(useDivsor) {
                        newVal /= 3;
                    }
                    monkeys[newVal % (BigInteger)monkey.Divisor == 0 ? monkey.TrueId : monkey.FalseId].Items.Add(newVal % monkeys.Select(m => m.Divisor).Aggregate(1, (m1, m2) => m1 * m2));
                }
                monkey.Items.RemoveAll(x => true);
            }
            //Console.WriteLine($"Round {i}");
            foreach (var monkey in monkeys) {
                //Console.WriteLine($"{monkey.Id}: {string.Join(',', monkey.Items)} | {monkey.InspectionCount}");
            }
        }
    }

    public static List<Monkey> GetMonkeys(string path) {
        var lines = File.ReadLines(path).ToList();
        var monkeys = new List<Monkey>();
        var enumerator = lines.GetEnumerator();
        while (enumerator.MoveNext()) {
            var monkeyId = int.Parse(Regex.Match(enumerator.Current, @"\d+").Value);
            enumerator.MoveNext();
            var items = Regex.Match(enumerator.Current, @"Starting items: (.+)$").Groups[1].Value
                .Split(", ")
                .Select(BigInteger.Parse)
                .ToList();
            enumerator.MoveNext();
            var operation = enumerator.Current.Split(" ").TakeLast(3).ToArray();
            enumerator.MoveNext();
            var divisor = int.Parse(enumerator.Current.Split(" ").Last());
            enumerator.MoveNext();
            var trueId = int.Parse(enumerator.Current.Split(" ").Last());
            enumerator.MoveNext();
            var falseId = int.Parse(enumerator.Current.Split(" ").Last());
            monkeys.Add(new Monkey (monkeyId, items, trueId, falseId, divisor, operation));
            enumerator.MoveNext();
        }
        return monkeys;
    }

    public class Monkey {
        public Monkey(int id, List<BigInteger> items, int trueId, int falseId, int divisor, string[] operation)
        {
            this.InspectionCount = 0;
            this.Divisor = divisor;
            this.Id = id;
            this.Items = items;
            this.TrueId = trueId;
            this.FalseId = falseId;
            this.Operation = operation;
        }

        public long InspectionCount {get;set;}
        public int Divisor {get;}
        public int Id {get;}
        public List<BigInteger> Items {get;}
        public int TrueId {get;}
        public int FalseId {get;}
        public string[] Operation {get;} 
    }
}