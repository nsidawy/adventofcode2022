internal class Program
{
    private static void Main(string[] args)
    {
        var lines = File.ReadLines("input1.txt").Skip(1);
        var root = new Directory("/", null);        
        ProcessLine(lines, root);
        Console.WriteLine(GetSizesUnder(root, 100000));

        var totalSpace = 70000000;
        var targetSpace = 30000000;
        var usedSpace = root.Size();
        var spaceToDelete = usedSpace - (totalSpace - targetSpace);
        var directoryToDelete = GetDirectoryToDelete(root, root, spaceToDelete);
        Console.WriteLine(directoryToDelete.Size());
    }

    private static Directory GetDirectoryToDelete(Directory current, Directory best, int spaceToDelete) {
        if(current.Size() > spaceToDelete && current.Size() < best.Size()){
            best = current;
        }
        foreach(var sub in current.SubDirectories.Values) {
            best = GetDirectoryToDelete(sub, best, spaceToDelete);
        }
        return best;
    }

    private static int GetSizesUnder(Directory directory, int max) {
        var size = directory.Size();
        var total = 0;
        if(size < max) {
            total += size;
        }
        foreach(var sub in directory.SubDirectories.Values){
            total += GetSizesUnder(sub, max);
        }

        return total;
    }

    private static void ProcessLine(IEnumerable<string> lines, Directory directory) 
    {
        if(!lines.Any()){
            return;
        }
        var command = lines.First();
        var args = command.Split(" ");
        var rest = lines.Skip(1);
        if (args[1] == "cd"){
            if(args[2] == ".."){
                ProcessLine(rest, directory.ParentDirectory);
            } else {
                ProcessLine(rest, directory.SubDirectories[args[2]]);
            }
        }
        else if(args[1] == "ls"){
            foreach (var line in rest.TakeWhile(r => r[0] != '$')){
                var output = line.Split(" ");
                if(output[0] == "dir"){
                    directory.SubDirectories[output[1]] = new Directory(output[1], directory);
                } else {
                    directory.Files.Add(new FileX(int.Parse(output[0]), output[1]));
                }
            }
            ProcessLine(rest.SkipWhile(r => r[0] != '$'), directory);
        }
    }

    private class Directory {
        private int? _size = null;

        public Directory(string name, Directory parent) {
            this.ParentDirectory = parent;
            this.Name = name;
            this.SubDirectories = new Dictionary<string, Directory>();
            this.Files = new List<FileX>();
        }

        public string Name {get;set;}

        public Directory ParentDirectory {get;set;}

        public Dictionary<string, Directory> SubDirectories {get; set;}

        public List<FileX> Files {get; set;}

        public int Size() {
            if(!this._size.HasValue) {
                this._size = this.Files.Sum(f => f.Size) + this.SubDirectories.Values.Select(d => d.Size()).Sum();
            }

            return this._size.Value;
        }
    }

    private class FileX {
        public FileX(int size, string name) {
            this.Size = size;
            this.Neme = name;
        }
        
        public int Size {get;set;}
        public string Neme {get;set;}
    }
}
