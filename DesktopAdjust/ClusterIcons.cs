using Accord.MachineLearning.Clustering;
using Accord.Math;
using DesktopIconsManipulator;
using System.Drawing;

class ClusterIcons
{
    public async Task Apply(List<DesktopIcon> DesktopIcons)
    {
        var embeddings = DesktopIcons.Select(x => x.Embedding.Select(y => (double)y).ToArray()).ToArray();

        // Convert the high dimensional embeddings to 2D using t-SNE
        var dimReduction = new TSNE()
        {
            NumberOfOutputs = 2,
            Perplexity = 5
        };

        double[][] _2dEmbeddings = dimReduction.Transform(embeddings);

        // Rescale the 2D embeddings to fit the screen
        var minX = _2dEmbeddings.Min(x => x[0]);
        var maxX = _2dEmbeddings.Max(x => x[0]);
        var minY = _2dEmbeddings.Min(x => x[1]);
        var maxY = _2dEmbeddings.Max(x => x[1]);

        // Scale to range [0, 1]
        var rescaledEmbeddings = _2dEmbeddings.Select(x => new float[] { (float)((x[0] - minX) / (maxX - minX)), (float)((x[1] - minY) / (maxY - minY)) }).ToArray();

        // Now we can use the desktop icons and their rescaled embeddings to display them on the screen by multiplying the rescaled embeddings by the screen width and height
        var iconSize = IconsManipulator.Instance.IconsSize;
        Rectangle bounds = new Rectangle(iconSize, iconSize, 2500 - iconSize, 1540 - iconSize);

        for (int i = 0; i < DesktopIcons.Count; i++)
        {
            var icon = DesktopIcons[i];
            var embedding = rescaledEmbeddings[i];

            var x = bounds.X + (int)(embedding[0] * bounds.Width);
            var y = bounds.Y + (int)(embedding[1] * bounds.Height);

            icon.Icon.SetItemPosition(new(x, y));
        }

        // Save the new desktop layout
        IconsManipulator.Instance.Refresh();

        Console.WriteLine("Desktop layout saved successfully!");

        // Wait for the user to press a key before closing the console
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}


