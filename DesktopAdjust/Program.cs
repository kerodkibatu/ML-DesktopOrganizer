using DesktopIconsManipulator;


Console.Write("How do you want to reorganize your desktop icons? (C)luster, (S)ort: ");
var choice = Console.ReadLine()!.ToLower();


var icons = IconsManipulator.Instance.Icons;
var DesktopIcons = await DesktopIcon.ToHighLevelIcons([.. icons]);

if (choice == "c")
{
    await new ClusterIcons().Apply(DesktopIcons);
}
else if (choice == "s")
{
    await new SortIcons().Apply(DesktopIcons);
}
else
{
    Console.WriteLine("Invalid choice. Exiting...");
}