using Core;
using Core.Exceptions;
using ProjectManager.Project;

namespace ProjectManager;

public static class CommandManager
{ 
    public static void Manage(string[] args)
    {
        try
        {
            switch (args)
            {
                case ["new", var name]:
                    New(name.Trim());
                    return;
                case ["run", ..]:
                    Run(args.Skip(1).ToList()!);
                    return;
                case ["update"]:
                    ConsoleLogger.Info("Qlang has the latest version");
                    return;
                case ["build"]:
                    Build();
                    return;
                case ["set", var param, var value]:
                    Set(param.Trim(), value.Trim());
                    return;
                case ["get", var param]:
                    Get(param.Trim());
                    return;
                case ["help"]:
                    Help();
                    return;
                case ["proj-info"]:
                    WriteProjectInfo();
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
        catch (QlangRuntimeException e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Runtime error: ");
            Console.ResetColor();
            Console.WriteLine(e);
            Thread.Sleep(2000);
        }
        catch (ProjectException e)
        {
            WriteErrorMessageWithDelay(e.Message);
        }
        catch (FileNotFoundException e)
        {
            WriteErrorMessageWithDelay(e.Message);
        }
        catch (DirectoryNotFoundException e)
        {
            WriteErrorMessageWithDelay(e.Message);
        }
        catch (Exception e)
        {
            ConsoleLogger.Info("UNHANDLED ERROR:");
            ExceptionManager.Throw(e, true);
            Thread.Sleep(2000);
        }
    }

    private static void WriteErrorMessageWithDelay(string message)
    {
        ExceptionManager.ThrowMessage(message);
        Thread.Sleep(2000);
    }

    private static Project.Project LoadProject()
    {
        var proj = Project.Project.LoadProject(Path.Combine(Directory.GetCurrentDirectory(),
            "project.settings.json"));

        return proj ?? throw new Exception("Project is not found");
    }

    private static void WriteProjectInfo()
    {
        var project = LoadProject();
        var props = project.Settings.GetDictionary();
        var compileProps = Project.Project.CompileSettings.GetDictionary();

        var names = new List<string>();
        var values = new List<string>();

        if (props is not null)
        {
            values.AddRange(props.Values.Select(var => var is null ? "<null>" : var.ToString()).ToList());
            names.AddRange(props.Keys.ToList());
        }

        if (compileProps is not null)
        {
            names.AddRange(compileProps.Keys.ToList());
            values.AddRange(compileProps.Values.Select(var => var is null ? "<null>" : var.ToString()).ToList());
        }

        if (names.Count != values.Count)
        {
            WriteErrorMessageWithDelay("Internal error: names count is not equal to values count in function 'proj-info'");
            return;
        }
        
        var output = new List<string>();
        for (var index = 0; index < names.Count; index++)
        {
            output.Add(names[index]);
            output.Add(values[index]);
        }

        
        Console.WriteLine($"""
                           Information about project: 
                           {TableCreator.Create([
                               ["Name", "Value"],
                               output
                           ])}
                           """);
    }

    private static void New(string name)
    {
        var proj = Project.Project.CreateProject(name, Directory.GetCurrentDirectory(), "main.ql");

        proj.SaveProject();

        ConsoleLogger.Info($"The project was created in the folder: '{proj.Settings.GetString("path")}'");
    }

    private static void Run(List<string?>? args)
    {
        LoadProject().Run(args);
    }

    private static void Build()
    {
        LoadProject().Compile();
        
        ConsoleLogger.Info("Project built successfully.");
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
            ["Guide book", "https://sotokata.github.io/Qlang-guide-book/"]
        ]));

        Console.WriteLine("Program information");
        Console.WriteLine(TableCreator.Create([
            ["Path", ": " + (Environment.ProcessPath ?? "Undefined")],
            ["Lib path", ": " + Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), "lib")],
        ]));

        ConsoleLogger.Info("Type 'ql help' to get information about available commands.");
    }
}