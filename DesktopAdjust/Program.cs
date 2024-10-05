using DesktopIconsManipulator;


Console.Write("How do you want to reorganize your desktop icons? (C)luster, (S)ort: ");
var choice = Console.ReadLine()!.ToLower();


var icons = IconsManipulator.Instance.Icons;

if (choice == "c")
{
    var DesktopIcons = await DesktopIcon.ToHighLevelIcons([.. icons]);

    new ClusterIcons().Apply(DesktopIcons);
}
else if (choice == "s")
{
    Console.Write("Enter the number of folders you want to create: ");
    var k = int.Parse(Console.ReadLine()!);

    var DesktopIcons = await DesktopIcon.ToHighLevelIcons([.. icons], k);
    await new SortIcons().Apply(DesktopIcons,k);
}
else
{
    Console.WriteLine("Invalid choice. Exiting...");
}