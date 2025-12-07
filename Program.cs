using System.Globalization;
using Qlang.Core.ProjectManager;

namespace Qlang;

public static class Program
{
    public static void Main(string[] args)
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

        CommandManager.Manage(args);
    }
}


