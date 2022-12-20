internal class Program
{
    private static void Main(string[] args)
    {
        var numbers = File.ReadLines("input.txt")
            .Select(l => new Node {Value = int.Parse(l)})
            .ToList();;
        Console.WriteLine(Calculate(numbers, 1, 1));
        Console.WriteLine(Calculate(numbers, 811589153, 10));
    }

    private static long Calculate(List<Node> numbers, int decryptionKey, int rounds) {
        numbers.ForEach(n => n.Value *= decryptionKey);
        var list = numbers.ToList();
        var length = list.Count;
        for (var i = 0; i < rounds; i++) {
            foreach (var number in numbers) {
                var index = list.IndexOf(number);
                list.RemoveAt(index);
                long newIndex;
                if(index + number.Value >= 0) {
                    newIndex = (index + number.Value) % (length - 1);
                } else {
                    newIndex = (length - 1) + ((index + number.Value) % (length - 1));
                }
                list.Insert((int)newIndex, number);
            }
        }
        var zeroIndex = list.FindIndex(x => x.Value == 0);
        return list[(zeroIndex + 1000) % length].Value
            + list[(zeroIndex + 2000) % length].Value
            + list[(zeroIndex + 3000) % length].Value;
    }

    private class Node {
        public long Value {get; set;}

        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}