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
            if(allPackets[i].CompareTo(allPackets[i+1]) < 0) {
                sum += (i/2) + 1;
            }
        }
        Console.WriteLine(sum);

        var six = new L(new List<Item> { new L(6) });
        var two = new L(new List<Item> { new L(2) });
        allPackets.Add(six);
        allPackets.Add(two);
        allPackets.Sort();
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

    public class Item : IComparable
    {
        public int CompareTo(object? obj)
        {
            var item = (Item)obj;
            if(this is V v1 && item is V v2) {
                return (v1.Value < v2.Value)
                    ?  -1 
                    : (v1.Value > v2.Value) 
                        ? 1
                        : 0;
            } else if(this is L l1 && item is L l2) {
                for (var i = 0; i < l1.Items.Count; i++){
                    if(i == l2.Items.Count) {
                        return 1;
                    }
                    var result = l1.Items[i].CompareTo(l2.Items[i]);
                    if(result != 0) {
                        return result;
                    }
                }
                return l1.Items.Count == l2.Items.Count ? 0 : -1;
            }

            var i1n = this is V v1n ? new L(v1n.Value) : this;
            var i2n = item is V v2n ? new L(v2n.Value) : item;
        
            return i1n.CompareTo(i2n);
        }
    }
    
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
}