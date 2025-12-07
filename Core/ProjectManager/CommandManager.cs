using Qlang.Core.Lang.Dynamic.Exceptions;
using Qlang.Core.ProjectManager.Project;

namespace Qlang.Core.ProjectManager;

public static class CommandManager
{ 
    public static void Manage(string[] args)
    {
        try
        {
            switch (args)
            {
                case ["new", _]:
                    New(args[1].Trim());
                    return;
                case ["run"]:
                    Run();
                    return;
                case ["update"]:
                    ConsoleLogger.Info("Qlang has the latest version");
                    return;
                case ["build"]:
                    Build();
                    return;
                case ["set", _, _]:
                    Set(args[1].Trim(), args[2].Trim());
                    return;
                case ["get", _]:
                    Get(args[1].Trim());
                    return;
                case ["help"]:
                    Help();
                    return;
                case []:
                    Qlang();
                    return;
                default:
                    ExceptionManager.ThrowMessage($"Unknown command: '{string.Join(" ", args)}'");
                    return;
            }
        }
        // catch (QlangCompileException e)
        // {
        //     Console.ForegroundColor = ConsoleColor.Red;
        //     Console.Write("Compile error: ");
        //     Console.ResetColor();
        //     Console.WriteLine(e);
        // }
        // catch (QlangRuntimeException e)
        // {
        //     Console.ForegroundColor = ConsoleColor.Red;
        //     Console.Write("Runtime error: ");
        //     Console.ResetColor();
        //     Console.WriteLine(e);
        // }
        catch (ProjectException e)
        {
            ExceptionManager.ThrowMessage(e.Message);
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
            ConsoleLogger.Info("Throw");
            ExceptionManager.Throw(e, true);
        }
    }

    private static Project.Project LoadProject()
    {
        var proj = Project.Project.LoadProject(Path.Combine(Directory.GetCurrentDirectory(),
            "project.settings.json"));

        return proj ?? throw new Exception("Project is not found");
    }

    private static void New(string name)
    {
        var proj = Project.Project.CreateProject(name, Directory.GetCurrentDirectory(), "main.ql");

        proj.SaveProject();

        ConsoleLogger.Info($"The project was created in the folder: '{proj.Settings.GetString("path")}'");
    }

    private static void Run()
    {
        LoadProject().Run();
    }

    private static void Build()
    {
        LoadProject().Compile();

        ConsoleLogger.Info("Project built successfully");
    }

    private static void Get(string param)
    {
        LoadProject();
        ConsoleLogger.Get($"{param}: {Project.Project.GetCompileSetting(param.Trim())}");
    }
    
    private static void Set(string param, string value)
    {
        LoadProject();
        Project.Project.SetCompileSetting(param.Trim(), value.Trim());
    }

    private static void Help()
    {
        Console.WriteLine("List of all commands:");
        TableCreator.WriteTable(new ConsoleTable(
            [
                [new TableCell("Commands"), new TableCell("Params"), new TableCell("Description")],
                [
                    new TableCell("new", ConsoleColor.Yellow),
                    new TableCell("[project_name]", ConsoleColor.DarkGray),
                    new TableCell("Creates new project with name 'name'")
                ],
                [
                    new TableCell("build", ConsoleColor.Yellow), new TableCell(""),
                    new TableCell("Build's existing project")
                ],
                [
                    new TableCell("run", ConsoleColor.Yellow), new TableCell(""),
                    new TableCell("Build's and run's existing project")
                ],
                [
                    new TableCell("set", ConsoleColor.Yellow),
                    new TableCell("[param] [value]", ConsoleColor.DarkGray),
                    new TableCell("Set param by value (compiler options)")
                ],
                [
                    new TableCell("get", ConsoleColor.Yellow),
                    new TableCell("[param]", ConsoleColor.DarkGray),
                    new TableCell("Get value of param by name (compiler options)")
                ],
            ]
        ));
    }

    private static void Qlang()
    {
        Console.WriteLine("Qlang information");
        Console.WriteLine(TableCreator.Create([
            ["Version", "0.10.1 'Project Update v0.1'"],
            ["Update date", "03.12.2025"],
            ["Author", "SOTOKATA (https://github.com/SOTOKATA)"],
            ["Github", "https://github.com/SOTOKATA/Qlang"],
            ["Guide book", "https://github.com/SOTOKATA/Qlang-guide-book"]
        ]));

        Console.WriteLine("Program information");
        Console.WriteLine(TableCreator.Create([
            ["Path", ": " + (Environment.ProcessPath ?? "Undefined")],
            ["Lib path", ": " + Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), "lib")],
        ]));

        ConsoleLogger.Info("Type 'ql help' to get information about available commands.");
    }
}