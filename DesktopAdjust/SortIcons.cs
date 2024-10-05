using Accord.MachineLearning;
using Accord.MachineLearning.Clustering;
using Accord.Math.Distances;
using DesktopAdjust.Services;

class SortIcons
{
    public async Task Apply(List<DesktopIcon> icons, int k = 5)
    {
        KMeans kmeans = new(k, distance: new Cosine());

        var embeddings = icons.Select(x => x.Embedding.Select(y => (double)y).ToArray()).ToArray();


        var dimReduction = new TSNE()
        {
            NumberOfOutputs = 2,
            Perplexity = 5
        };

        double[][] _2dEmbeddings = dimReduction.Transform(embeddings);

        var clusters = kmeans.Learn(_2dEmbeddings);

        for (int i = 0; i < icons.Count; i++)
        {
            var icon = icons[i];
            var embedding = _2dEmbeddings[i];
            var cluster = clusters.Decide(embedding);
            icon.Cluster = cluster;
        }

        var IconsByCluster = icons.GroupBy(x => x.Cluster).ToDictionary(x => x.Key, x => x.ToList());


        var conv = Completions.NewConversation();

        conv.AppendSystemMessage(
        $"""
            My Desktop Consists of the following: {string.Join(", ", icons.Select(x => x.Icon.Name))}
            """);

        List<string> movedItems = [];

        string Desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

        foreach (var cluster in IconsByCluster)
        {
            // Label the cluster
            conv.AppendUserInput(
                $"""
                Give me a fitting name for a folder that contains the following files/shortcuts.

                Respond with just the folder name, Nothing Else. No Quotations, No Additional Options. Try Making it 1 Word.

                Icons:
                {string.Join('\n', cluster.Value)}
                """);

            var Title = await conv.GetResponseFromChatbotAsync();

            Console.WriteLine($"Cluster {Title}:");
            foreach (var icon in cluster.Value)
            {
                try
                {
                    bool isShortcut = !Directory.Exists(icon.Icon.FullPath);
                    var folderPath = Path.Combine(Desktop, Title);
                    Directory.CreateDirectory(folderPath);

                    Console.WriteLine(icon.Icon.Name);

                    var the_file = icon.Icon.FullPath;

                    if (Path.Exists(the_file + ".lnk"))
                        the_file += ".lnk";
                    else if (Path.Exists(the_file + ".url"))
                        the_file += ".url";

                    var dest = isShortcut ? Path.Combine(folderPath, Path.GetFileName(the_file)) : Path.Combine(folderPath, icon.Icon.Name);

                    if (isShortcut)
                    {
                            File.Move(the_file, dest);
                    }
                    else
                    {
                        if (Path.Exists(the_file))
                            Directory.Move(the_file, dest + '\\');
                    }

                    movedItems.Add(dest);
                }
                catch
                {
                    Console.WriteLine("Error moving file, Skipping....");
                }
            }
            Console.WriteLine();
        }

        // Press any key to revert, press enter to keep changes
        Console.WriteLine("Press any key to revert changes, press enter to keep changes");
        var key = Console.ReadKey();
        if (key.Key == ConsoleKey.Enter)
        {
            Console.WriteLine("Changes Saved");
        }
        else
        {
            foreach (var item in movedItems)
            {
                var dest = Path.Combine(Desktop, Path.GetFileName(item));

                try
                {
                    if (Path.Exists(item))
                        Directory.Move(item, dest);
                }
                catch
                {
                    Console.WriteLine("Error reverting file, Skipping....");
                }
            }

            // Then delete all empty folders
            foreach (var folder in Directory.GetDirectories(Desktop))
            {
                if (Directory.GetFiles(folder).Length == 0 && Directory.GetDirectories(folder).Length == 0)
                    Directory.Delete(folder);
            }
            Console.WriteLine("Changes Reverted");
        }
    }
}
