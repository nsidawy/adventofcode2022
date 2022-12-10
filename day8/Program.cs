internal class Program
{
    private static void Main(string[] args)
    {
        var trees = File.ReadLines("input1.txt")
            .Select(l => l.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray())
            .ToArray();
        var visibleCount = trees
            .Select((row, i) => row
                .Where((_, j) => IsVisible(trees, i, j))
                .Count())
            .Sum();
        Console.WriteLine(visibleCount);

        var maxScenicScore = trees.Select((row, i) => row.
                Select((_, j) => GetScenicScore(trees, i, j))
                .Max())
            .Max();
        Console.WriteLine(maxScenicScore);
    }

    private static int GetScenicScore(int[][] trees, int x, int y){
        var current = trees[x][y];
        var score1 = 0;
        for(var i = x; i > 0; i--) {
            score1++;
            if(trees[i-1][y] >= current){
                break;
            }
        }
        var score2 = 0;
        for(var i = x; i < trees.Length -1; i++) {
            score2++; 
            if(trees[i+1][y] >= current) {
                break;
            }
        }
        var score3 = 0;
        for(var j = y; j > 0; j--) {
            score3++;
            if(trees[x][j-1] >= current) {
                break;
            }
        }
        var score4 = 0;
        for(var j = y; j < trees[0].Length - 1; j++) {
            score4++;
            if(trees[x][j+1] >= current) {
                break;
            }
        }
        return score1 * score2 * score3 * score4;
    }

    private static bool IsVisible(int[][] trees, int x, int y){
        var current = trees[x][y];
        for(var i = x; i >= 0; i--) {
            if(i == 0) {
                return true;
            }
            if(trees[i-1][y] >= current) {
                break;
            }
        }
        for(var i = x; i < trees.Length; i++) {
            if(i == trees.Length - 1) {
                return true;
            }
            if(trees[i+1][y] >= current) {
                break;
            }
        }
        for(var j = y; j >= 0; j--) {
            if(j == 0) {
                return true;
            }
            if(trees[x][j-1] >= current) {
                break;
            }
        }
        for(var j = y; j < trees[0].Length; j++) {
            if(j == trees.Length - 1) {
                return true;
            }
            if(trees[x][j+1] >= current) {
                break;
            }
        }

        return false;
    }
}
