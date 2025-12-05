using System.Globalization;
using Qlang.Dependencies;

namespace Qlang;

public class Program
{
    public static void Main(string[] args)
    {
        // Console.WriteLine($"Directory: {Directory.GetCurrentDirectory()}");
        // to double write with dot (not comma)
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

        Project.Project proj;
        try
        {
            switch (args)
            {
                case ["new", _]:
                    string name = args[1].Trim();

                    proj = Project.Project.CreateProject(name, Directory.GetCurrentDirectory(), "main.ql");

                    proj.SaveProject();

                    return;
                case ["run"]:
                    proj = Project.Project.LoadProject(Path.Combine(Directory.GetCurrentDirectory(),
                        "project.settings.json"));

                    if (proj is null)
                        throw new Exception("Project is not found");

                    proj.Run();
                    return;
                case ["build"]:
                    proj = Project.Project.LoadProject(Path.Combine(Directory.GetCurrentDirectory(),
                        "project.settings.json"));

                    if (proj is null)
                        throw new Exception("Project is not found");

                    proj.Compile();
                    return;
                case ["set", _, _]:
                    proj = Project.Project.LoadProject(Path.Combine(Directory.GetCurrentDirectory(),
                        "project.settings.json"));

                    if (proj is null)
                        throw new Exception("Project is not found");
                    
                    Project.Project.SetCompileSetting(args[1].Trim(), args[2].Trim());
                    return;
                case ["get", _]:
                    proj = Project.Project.LoadProject(Path.Combine(Directory.GetCurrentDirectory(),
                        "project.settings.json"));

                    if (proj is null)
                        throw new Exception("Project is not found");

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("GET");
                    Console.ResetColor();
                    Console.WriteLine($" {args[1]}: {Project.Project.GetCompileSetting(args[1].Trim())}");
                    return;
                case ["help"]:
                    Console.WriteLine("List of all commands:");
                    TableCreator.WriteTable(new ConsoleTable(
                        [
                            [new TableCell("Commands"), new TableCell("Params"), new TableCell("Description")],
                            [new TableCell("new", ConsoleColor.Yellow), new TableCell("[project_name]", ConsoleColor.DarkGray), new TableCell("Creates new project with name 'name'")],
                            [new TableCell("build", ConsoleColor.Yellow), new TableCell(""), new TableCell("Build's existing project")],
                            [new TableCell("run", ConsoleColor.Yellow), new TableCell(""), new TableCell("Build's and run's existing project")],
                            [new TableCell("set", ConsoleColor.Yellow), new TableCell("[param] [value]", ConsoleColor.DarkGray), new TableCell("Set param by value (compiler options)")],
                            [new TableCell("get", ConsoleColor.Yellow), new TableCell("[param]", ConsoleColor.DarkGray), new TableCell("Get value of param by name (compiler options)")],
                        ]
                        ));
                    return;
                case []:
                    Console.WriteLine("Qlang information");
                    Console.WriteLine(TableCreator.Create([
                        ["", ""],
                        ["Version", "0.10.1 'Project Update v0.1'"],
                        ["Update date", "03.12.2025"],
                        ["Author", "SOTOKATA (https://github.com/SOTOKATA)"],
                        ["Github", "https://github.com/SOTOKATA/Qlang"],
                        ["Guide book", "https://github.com/SOTOKATA/Qlang-guide-book"]
                        ]));
                    
                    Console.WriteLine("Program information");
                    Console.WriteLine(TableCreator.Create([
                        ["", ""],
                        ["Path", (Environment.ProcessPath ?? "Undefined")],
                        ["Lib path", Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), "lib")],
                    ], [":"]));
                    return;
                default:
                    ExceptionManager.ThrowMessage($"Unknown command: '{string.Join(" ", args)}'");
                    return;
            }
        }
        catch (FileNotFoundException e)
        {
            ExceptionManager.ThrowMessage(e.Message);
        }
        catch (DirectoryNotFoundException e)
        {
            ExceptionManager.ThrowMessage(e.Message);
        }
        catch (Exception e)
        {
            ExceptionManager.Throw(e);
        }
    }
}


