internal class Program
{
    private static void Main(string[] args)
    {
        var lines = File.ReadLines("input.txt").ToList();
        var sum = 0;
        var itemComparer = new ItemComparer();
        var allPackets = lines
            .Where(l => l != string.Empty)
            .Select(l => {
               var (list, _) = ParseList(l, 0);
               return list;
            })
            .ToList();
        for (var i = 0; i < allPackets.Count; i += 2) {
            if(itemComparer.Compare(allPackets[i], allPackets[i+1]) < 0) {
                sum += (i/2) + 1;
            }
        }
        Console.WriteLine(sum);

        var six = new L(new List<Item> { new L(6) });
        var two = new L(new List<Item> { new L(2) });
        allPackets.Add(six);
        allPackets.Add(two);
        allPackets.Sort(itemComparer);
        var sixIndex = allPackets.IndexOf(six) + 1;
        var twoIndex = allPackets.IndexOf(two) + 1;
        Console.WriteLine(twoIndex * sixIndex);
    }

    public static (L, int) ParseList(string l, int position){
        var list = new L();
        var i = position + 1;
        while(i < l.Length){
            if (l[i] == ']'){
                return (list, i + 1);
            } else if (l[i] == ',') {
                i++; 
            } else if (l[i] == '[') {
                var (sublist, newPosition) = ParseList(l, i);
                list.Items.Add(sublist);
                i = newPosition;
           } else {
                var num = string.Empty;
                while(int.TryParse(l[i].ToString(), out int value)) {
                    num += l[i];
                    i++;
                }
                list.Items.Add(new V(int.Parse(num)));
            }
        }
        return (new L(), -1);
    }

    public interface Item {}
    public class L : Item {
        public L() {
            this.Items = new List<Item>();
        }
        public L(int v) {
            this.Items = new List<Item> { new V (v) };
        }
        public L(List<Item> sublist) {
            this.Items = sublist;
        }
        public List<Item> Items {get;}
    }
    public class V : Item {
        public V(int value) {
            this.Value = value;
        }
        public int Value {get;}
    }

    public class ItemComparer : IComparer<Item>
    {
        public int Compare(Item? i1, Item? i2)
        {
            if(i1 is V v1 && i2 is V v2) {
                return (v1.Value < v2.Value)
                    ?  -1 
                    : (v1.Value > v2.Value) 
                        ? 1
                        : 0;
            } else if(i1 is L l1 && i2 is L l2) {
                for (var i = 0; i < l1.Items.Count; i++){
                    if(i == l2.Items.Count) {
                        return 1;
                    }
                    var result = this.Compare(l1.Items[i], l2.Items[i]);
                    if(result != 0) {
                        return result;
                    }
                }
                return l1.Items.Count == l2.Items.Count ? 0 : -1;
            }

            var i1n = i1 is V v1n ? new L(v1n.Value) : i1;
            var i2n = i2 is V v2n ? new L(v2n.Value) : i2;
        
            return this.Compare(i1n, i2n);
        }
    }
}