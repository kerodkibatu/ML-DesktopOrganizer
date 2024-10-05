using DesktopAdjust.Services;
using DesktopIconsManipulator;
using ShellProgressBar;
using System.Diagnostics;

public record DesktopIcon(IconItem Icon, string Description, float[] Embedding)
{
    public int Cluster { get; set; }
    public static async Task<List<DesktopIcon>> ToHighLevelIcons(List<IconItem> icons, int k = -1)
    {
        List<DesktopIcon> desktopIcons = [];

        var conv = Completions.NewConversation();
        conv.AppendSystemMessage(
            $"""
            I will now ask you to describe what each shortcut on my desktop is used for or what it is about.

            {(k != -1 ? $"ONLY RESPOND WITH {k} DISCRETE CATAGORY THE APP FALLS TO AND NOTHING ELSE":"")}

            shortcut on the desktop:
            {string.Join("\n", icons.Select(x => $"{x.Name} ({(x.IsFile ? "File" : "Folder")})"))}
            """);

        using (var p = new ProgressBar(icons.Count, "Processing desktop files...", new ProgressBarOptions()
        {
            ShowEstimatedDuration = true,
        }))
        {
            Stopwatch sw = Stopwatch.StartNew();
            foreach (var icon in icons)
            {
                conv.AppendUserInput(
                    $$"""
                    Describe the shortcut '{{icon.Name}}'.

                    RESPOND IN THE FOLLOWING FORMAT, NOTHING ELSE
                    {GENERAL GENRE},{SPECIFIC GENRE}
                    
                    """);
                var desc = icon.Name + "\n";
                desc += await conv.GetResponseFromChatbotAsync();

                var embedding = await Embeddings.EmbedText(desc);

                desktopIcons.Add(new DesktopIcon(icon, desc, embedding));
                var estTime = TimeSpan.FromSeconds(sw.Elapsed.TotalSeconds + sw.Elapsed.TotalSeconds / (desktopIcons.Count + 1) * (icons.Count - desktopIcons.Count));
                p.Tick(estTime, $"Processed {desktopIcons.Count}/{icons.Count} icons.");
            }
        }


        return desktopIcons;
    }
}