using Core.Exceptions;
using ProjectManager.Project;
using ProjectManager.Settings;

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
                    ConsoleLogger.Info("Now, this function is not supported");
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
                case ["info"]:
                    WriteProjectInfo();
                    return;
                case ["echo", ..]:
                    ConsoleLogger.Info(string.Join(" ", args.Skip(1)));
                    return;
                case []:
                    Qlang();
                    return;
                default:
                    ExceptionManager.ThrowMessage($"""
                                                   Unknown command: '{string.Join(" ", args)}'
                                                   To get information about commands, type 'ql help'.
                                                   """);
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
            ConsoleLogger.Log("Runtime error: ", e.ToString(), ConsoleColor.Red);
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
            ProjectSettings.JsonFileName));

        return proj ?? throw new ProjectException("Project is not found");
    }

    private static void WriteProjectInfo()
    {
        var project = LoadProject();
        var props = project.GetProjectSettings().GetDictionary();
        var compileProps = project.GetCompileSettings().GetDictionary();

        var names = new List<string>();
        var values = new List<string>();

        if (props is not null)
        {
            values.AddRange(props.Values.Select(var => var.@object is null ? "<null>" : var.@object.ToString()).ToList()!);
            names.AddRange(props.Keys.ToList());
        }

        if (compileProps is not null)
        {
            names.AddRange(compileProps.Keys.ToList());
            values.AddRange(compileProps.Values.Select(var => var.@object is null ? "<null>" : var.@object.ToString()).ToList()!);
        }

        if (names.Count != values.Count)
        {
            WriteErrorMessageWithDelay("Internal error: names count is not equal to values count in command 'info'");
            return;
        }
        
        var table = new List<List<string>>
        {
            new() { "Parameter name", "Parameter value" }
        };
        table.AddRange(names.Select((t, index) => (List<string>)[t, values[index]]));


        Console.WriteLine($"""
                           Information about '{project.GetProjectSetting(ProjectSettings.ProjectName)}':
                           {TableCreator.Create(table, [":"])}
                           """);
    }

    private static void New(string name)
    {
        var proj = Project.Project.CreateProject(name, Directory.GetCurrentDirectory(), "main.ql");

        proj.SaveProject();

        ConsoleLogger.Info($"The project was created in the folder: '{proj.GetProjectSetting(ProjectSettings.RootPath)}'");
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
        ConsoleLogger.Get($"{param}: {LoadProject().GetCompileSetting(param.Trim())}");
    }
    
    private static void Set(string param, string value)
    {
        var project = LoadProject();
        project.SetCompileSetting(param.Trim(), value.Trim());
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
                [
                    new TableCell("info", ConsoleColor.Yellow),
                    new TableCell(""),
                    new TableCell("Write's information about current project")
                ]
            ]
        ));
    }

    private static void Qlang()
    {
        Console.WriteLine("Qlang information:");
        Console.WriteLine(TableCreator.Create([
            ["Author", "SOTOKATA (https://github.com/SOTOKATA)"],
            ["Github", "https://github.com/SOTOKATA/Qlang"],
            ["Guide book", "https://sotokata.github.io/Qlang-guide-book/"]
        ]));

        Console.WriteLine("Program information:");
        Console.WriteLine(TableCreator.Create([
            ["Path", ": " + (Environment.ProcessPath ?? "Undefined")],
            ["Lib path", ": " + Path.Combine(Path.GetDirectoryName(Environment.ProcessPath)!, "lib")]
        ]));

        ConsoleLogger.Info("Type 'ql help' to get information about available commands.");
    }
}