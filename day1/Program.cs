internal class Program
{
    private static void Main(string[] args)
    {
        var path = "input1.txt";
        var calorieList = GetCalorieLists(path); 
        var max = calorieList.Max();
        Console.WriteLine(max);
        var topThree = calorieList
            .OrderDescending()
            .Take(3)
            .Sum();
        Console.WriteLine(topThree);
    }

    private static List<int> GetCalorieLists(string path)
    {
        var lines = File.ReadLines(path);
        var calorieList = new List<int>();
        var count = 0;
        foreach (var line in lines){
            if (line == "") {
                calorieList.Add(count);
                count = 0;
            }
            else {
                count += int.Parse(line);
            }
        }

        return calorieList;
    }
}