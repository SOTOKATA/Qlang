using System.Globalization;

namespace ProjectManager;

public static class Program
{
    public static void Main(string[] args)
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

        CommandManager.Manage(args);
    }
}