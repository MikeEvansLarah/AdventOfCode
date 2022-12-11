using System.Xml.Linq;

namespace AdventOfCode.Solutions.Year2022.Day07;

class Solution : SolutionBase
{
    public Solution() : base(07, 2022, "No Space Left On Device") { }

    public enum ItemType
    {
        File,
        Directory
    }

    public class Item
    {
        public Item? Parent { get; set; }

        public Dictionary<string, Item> Children { get; set; } = new Dictionary<string, Item>();

        public int? Size { get; set; }

        public string? Name { get; set; }

        public ItemType Type { get; set; }

        public int TotalSize => this.Size ?? this.Children.Values.Sum(x => x.TotalSize);

        public IEnumerable<Item> Flatten()
        {
            yield return this;
            if (this.Children.Any())
            {
                foreach (var child in this.Children.Values)
                    foreach (var descendant in child.Flatten())
                        yield return descendant;
            }
        }
    }

    public class FileSystem
    {
        public Item Root { get; set; }

        public FileSystem(Item root)
        {
            Root = root;
        }

        public static FileSystem Parse(string input)
        {
            var fs = new FileSystem(new Item
            {
                Name = "/",
                Type = ItemType.Directory
            });

            var currentItem = fs.Root;

            foreach (string line in input.SplitByNewline().Skip(1))
            {
                if (line[0] == '$')
                {
                    if (line[2..4] == "cd")
                    {
                        var moveTo = line[5..];

                        if (moveTo == "..")
                        {
                            currentItem = currentItem!.Parent;
                        }
                        else
                        {
                            currentItem = currentItem!.Children[moveTo];
                        }
                    }
                }
                else if (line[0..3] == "dir")
                {
                    var directoryName = line[4..];
                    currentItem!.Children.Add(directoryName, new Item
                    {
                        Name = directoryName,
                        Type = ItemType.Directory,
                        Parent = currentItem
                    });
                }
                else
                {
                    var size = int.Parse(line.Split(' ')[0]);
                    var fileName = line.Split(' ')[1];

                    currentItem!.Children.Add(fileName, new Item
                    {
                        Name = fileName,
                        Type = ItemType.File,
                        Parent = currentItem,
                        Size = size
                    });
                }
            }

            return fs;
        }
    }

    protected override string SolvePartOne()
    {
        var fs = FileSystem.Parse(this.Input!);

        var flat = fs.Root.Flatten()
            .Where(x => x.Type == ItemType.Directory).ToList();

        var result = flat.Where(x => x.TotalSize <= 100000)
            .Sum(x => x.TotalSize);

        return result.ToString();
    }

    protected override string SolvePartTwo()
    {
        var fs = FileSystem.Parse(this.Input!);

        var unusedSpace = 70000000 - fs.Root.TotalSize;
        var minDeletionSize = 30000000 - unusedSpace;

        var flat = fs.Root.Flatten()
            .Where(x => x.Type == ItemType.Directory).ToList();

        var result = flat.Where(x => x.TotalSize >= minDeletionSize).MinBy(x => x.TotalSize)!.TotalSize;

        return result.ToString();
    }
}
