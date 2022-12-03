internal class Program
{
    private static void Main(string[] args)
    {
        var path = "input1.txt";
        var calorieList = GetCalorieLists(path); 
        var max = calorieList.Select(x => x.Sum()).Max();
        Console.WriteLine(max);
        var topThree = calorieList
            .Select(x => x.Sum())
            .OrderDescending()
            .Take(3)
            .Sum();
        Console.WriteLine(topThree);
    }

    private static List<List<int>> GetCalorieLists(string path)
    {
        var lines = File.ReadLines(path);
        var calorieList = new List<List<int>>();
        var currentList = new List<int>();
        foreach (var line in lines){
            if (line == "") {
                calorieList.Add(currentList);
                currentList = new List<int>();
            }
            else {
                currentList.Add(int.Parse(line));
            }
        }

        return calorieList;
    }
}