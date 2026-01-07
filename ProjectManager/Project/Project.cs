using Core.Debug;
using Newtonsoft.Json;
using ProjectManager.Settings;

namespace ProjectManager.Project;

public partial class Project
{
    public ProjectSettings Settings;

    public static CompileSettings? CompileSettings;

    private readonly QLang _qlang = new();

    public void SaveProject()
    {
        if (!File.Exists(Settings.GetPath()))
            File.Create(Settings.GetPath()).Close();

        Settings.Save();
    }

    public static Project CreateProject(string projectName, string projectPath, string mainFilePath)
    {
        if (!Directory.Exists(projectPath))
            throw new DirectoryNotFoundException($"Directory '{projectPath}' not found");

        var proj = new Project();

        projectPath = Path.Combine(projectPath, projectName);
        Directory.CreateDirectory(projectPath);

        File.Create(Path.Combine(projectPath, "project.settings.json")).Close();
        proj.Settings = new ProjectSettings(Path.Combine(projectPath, "project.settings.json"), null);
    
        proj.Settings.Set("path", projectPath);
        proj.Settings.Set("name", projectName);
        proj.Settings.Set("main_file_path", mainFilePath);
    
        // create main.ql
        var fullMainFilePath = Path.Combine(projectPath, mainFilePath);
        File.Create(fullMainFilePath).Close();
        File.WriteAllText(fullMainFilePath, """
                                            include "$lib/base"

                                            function main(): {
                                                Console.println("Hello World!");
                                            }
                                            """);
    
        proj.SaveProject();

        var settingsPath = Path.Combine(projectPath, "compile.settings.json");
        File.Create(settingsPath).Close();
        CompileSettings = new CompileSettings(settingsPath, null);
    
        CompileSettings.Save();
        
        return proj;
    }

    // path to settings file
    public static Project LoadProject(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"File '{path}' is not found.\nThe project may be corrupted or not created.", path);

        var settings = new ProjectSettings(path, ProjectManager.Settings.Settings.LoadDictionary(path));
        
        settings.Set("path", Directory.GetCurrentDirectory());
        var projectPath = Directory.GetCurrentDirectory();
        var mainFilePath = settings.GetString("main_file_path");
                
        if (
            !File.Exists(Path.Combine(projectPath, mainFilePath)) ||
            !File.Exists(Path.Combine(projectPath, "compile.settings.json"))
        )
            throw new FileNotFoundException($"Project is corrupted or not created.\nPath to project settings: '{path}'.", path);
        
        var proj = new Project
        {
            Settings = settings
        };

        var settingsPath = Path.Combine(projectPath, "compile.settings.json");
        var dict =  JsonConvert.DeserializeObject<Dictionary<string, object?>>(File.ReadAllText(settingsPath));
        CompileSettings = new CompileSettings(settingsPath, dict);

        var isDebug = (bool)CompileSettings.Get("debug_mode");
        Logger.Debug = isDebug;
        FileLogger.Debug = isDebug;
        
        return proj;
    }
}