using System.Globalization;

namespace Qlang;

public class Program
{
    public static void Main(string[] args)
    {
        // Console.WriteLine($"Directory: {Directory.GetCurrentDirectory()}");
        // to double write with dot (not comma)
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

        Project.Project proj;

        switch (args)
        {
            case ["new", _]:
                string name = args[1].Trim();
                
                proj = new Project.Project(name, Directory.GetCurrentDirectory(), Path.Combine(Directory.GetCurrentDirectory(), "main.ql"));
                
                proj.SaveProject();
                
                return;
            case ["run"]:
                proj = Project.Project.LoadProject(Path.Combine(Directory.GetCurrentDirectory(), "project.settings.json"));

                if (proj is null)
                    throw new Exception("Project is not found");

                proj.Run();
                return;
            case ["build"]:
                proj = Project.Project.LoadProject(Path.Combine(Directory.GetCurrentDirectory(), "project.settings.json"));

                if (proj is null)
                    throw new Exception("Project is not found");

                proj.Compile();
                return;
            case ["set", _, _]:
                proj = Project.Project.LoadProject(Path.Combine(Directory.GetCurrentDirectory(), "project.settings.json"));

                if (proj is null)
                    throw new Exception("Project is not found");

                Project.Project.SetCompileSetting(args[1].Trim(),  args[2].Trim());
                return;
            case  ["get", _]:
                proj = Project.Project.LoadProject(Path.Combine(Directory.GetCurrentDirectory(), "project.settings.json"));

                if (proj is null)
                    throw new Exception("Project is not found");

                Console.WriteLine($"GET {args[1]}: {Project.Project.GetCompileSetting(args[1].Trim())}");
                return;
            case ["help"]:
                Console.WriteLine("""
                                  # Commands
                                  > new [name]
                                  Creates new project with name 'name'
                                  
                                  > build
                                  Build's existing project
                                  
                                  > run
                                  Build's and run's existing project
                                  
                                  > set [param] [value]
                                  Set param by value (compiler options)
                                  
                                  > get [param]
                                  Get value of param by name (compiler options)
                                  """);
                return;
            case []:
                Console.WriteLine($"""
                                  # Qlang info
                                  Version    : 0.10.1 'Project Update v0.1'
                                  Update date: 02.12.2025
                                  Author     : SOTOKATA (https://github.com/SOTOKATA)
                                  Github     : https://github.com/SOTOKATA/Qlang
                                  Guide book : https://github.com/SOTOKATA/Qlang-guide-book
                                  
                                  # Program info
                                  Path    : {Environment.ProcessPath} 
                                  Lib path: {Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), "lib")}
                                  """);
                return;
            default:
                ExceptionManager.ThrowMessage($"Unknown command: '{string.Join(" ", args)}'");
                return;
        }
    }
}


