internal class Program
{
    private static void Main(string[] args)
    {
        var path = "input.txt";
        var monkeys = GetMonkeys(path);
        var result = monkeys["root"].Calculate(monkeys);
        Console.WriteLine(result);

        string current;
        long target;
        var arg1 = ((Operation)monkeys["root"]).Arg1;
        var arg2 = ((Operation)monkeys["root"]).Arg2;
        var rootAarg1HasHuman = monkeys[arg1].HasHuman(monkeys);
        if(rootAarg1HasHuman) {
            target = monkeys[arg2].Calculate(monkeys);
            current = monkeys[arg1].Name;
        } else {
            target = monkeys[arg1].Calculate(monkeys);
            current = monkeys[arg2].Name;
        }
        while(current != "humn") {
            var currentOp = ((Operation)monkeys[current]);
            var arg1HasHuman = monkeys[currentOp.Arg1].HasHuman(monkeys);
            var arg2HasHuman = monkeys[currentOp.Arg2].HasHuman(monkeys);
            current = arg1HasHuman ? currentOp.Arg1 : currentOp.Arg2;
            target = currentOp.Op switch {
                '+' => target - (arg1HasHuman 
                    ? monkeys[currentOp.Arg2].Calculate(monkeys)
                    : monkeys[currentOp.Arg1].Calculate(monkeys)),
                '-' => arg1HasHuman 
                    ? target + monkeys[currentOp.Arg2].Calculate(monkeys)
                    : -1 * (target - monkeys[currentOp.Arg1].Calculate(monkeys)),
                '*' => target / (arg1HasHuman 
                    ? monkeys[currentOp.Arg2].Calculate(monkeys)
                    : monkeys[currentOp.Arg1].Calculate(monkeys)),
                '/' => arg1HasHuman 
                    ? target * monkeys[currentOp.Arg2].Calculate(monkeys)
                    : (long)Math.Pow((double)(target / monkeys[currentOp.Arg1].Calculate(monkeys)), 2)
                };
        }
        Console.WriteLine(target);
    }

    private static Dictionary<string, Monkey> GetMonkeys(string path) {
        return File.ReadLines(path)
            .Select<string, Monkey>(l => {
                var parts = l.Split(':').ToArray();
                var name = parts[0];
                var expression = parts[1].Trim().Split(" ");
                if(expression.Length == 1) {
                    return new Number { Name = name, Value = int.Parse(expression[0])};
                } else {
                    return new Operation { Name = name, Op = expression[1][0], Arg1 = expression[0], Arg2 = expression[2]};
                }
            }).ToDictionary(x => x.Name, x => x);
    }

    private abstract class Monkey {
        public string Name {get; set;}
        public abstract long Calculate(Dictionary<string, Monkey> monkeys);
        public abstract bool HasHuman(Dictionary<string, Monkey> monkeys);
    }

    private class Number : Monkey {
        public long Value {get; set;}
        public override long Calculate(Dictionary<string, Monkey> monkeys)
        {
            return this.Value;
        }
        public override bool HasHuman(Dictionary<string, Monkey> monkeys)
        {
            return this.Name == "humn";
        }
    }

    private class Operation : Monkey {
        public char Op {get;set;}
        public string Arg1 {get;set;} = default!;
        public string Arg2 {get;set;} = default!;
        private int? Value { get; set;}
        public override long Calculate(Dictionary<string, Monkey> monkeys)
        {
            var arg1Value = monkeys[this.Arg1].Calculate(monkeys);
            var arg2Value = monkeys[this.Arg2].Calculate(monkeys);
            return this.Op switch {
                '+' => arg1Value + arg2Value,
                '*' => arg1Value * arg2Value,
                '-' => arg1Value - arg2Value,
                '/' => arg1Value / arg2Value,
            };
        }
        public override bool HasHuman(Dictionary<string, Monkey> monkeys)
        {
            return monkeys[this.Arg1].HasHuman(monkeys)
                || monkeys[this.Arg2].HasHuman(monkeys);
        }
    }
}