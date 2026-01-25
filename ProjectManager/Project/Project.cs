using Core.Debug;
using Newtonsoft.Json;
using ProjectManager.Settings;

namespace ProjectManager.Project;

public partial class Project(ProjectSettings projectSettings, CompileSettings compileSettings)
{
    private readonly ProjectSettings _settings = projectSettings;

    private readonly CompileSettings _compileSettings = compileSettings;

    private readonly QLang _qlang = new();

    public ProjectSettings GetProjectSettings() => _settings;
    public CompileSettings GetCompileSettings() => _compileSettings;

    public object? GetProjectSetting(string param) => _settings.Get(param);

    public void SaveProject()
    {
        if (!File.Exists(_settings.GetPath()))
            File.Create(_settings.GetPath()).Close();

        if (!File.Exists(_compileSettings.GetPath()))
            File.Create(_compileSettings.GetPath()).Close();
        
        _settings.Save();
        _compileSettings.Save();
    }

    public static Project CreateProject(string projectName, string projectPath, string mainFilePath)
    {
        if (!Directory.Exists(projectPath))
            throw new DirectoryNotFoundException($"Directory '{projectPath}' not found");
        
        projectPath = Path.Combine(projectPath, projectName);
        Directory.CreateDirectory(projectPath);

        File.Create(Path.Combine(projectPath, ProjectSettings.JsonFileName)).Close();
        var settings = new ProjectSettings(Path.Combine(projectPath, ProjectSettings.JsonFileName), null);
    
        settings.Set(ProjectSettings.RootPath, projectPath);
        settings.Set(ProjectSettings.ProjectName, projectName);
        settings.Set(ProjectSettings.MainFilePath, mainFilePath);
    
        // create main.ql
        var fullMainFilePath = Path.Combine(projectPath, mainFilePath);
        File.Create(fullMainFilePath).Close();
        File.WriteAllText(fullMainFilePath, """
                                            import "$lib/standard"
                                            using std;

                                            function main(): {
                                                Console.println("Hello World!");
                                            }
                                            """);
    
        var compileSettingsPath = Path.Combine(projectPath, CompileSettings.JsonFileName);
        File.Create(compileSettingsPath).Close();
        var proj = new Project(settings, new CompileSettings(compileSettingsPath, null));
        proj.SaveProject();

        return proj;
    }

    // path to settings file
    public static Project LoadProject(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"File '{path}' is not found.\nThe project may be corrupted or not created.", path);

        var settings = new ProjectSettings(path, ProjectManager.Settings.Settings.LoadDictionary<ProjectSettings>(path));
        
        settings.Set(ProjectSettings.RootPath, Directory.GetCurrentDirectory());
        var projectPath = Directory.GetCurrentDirectory();
        var mainFilePath = settings.GetString(ProjectSettings.MainFilePath);
                
        if (
            !File.Exists(Path.Combine(projectPath, mainFilePath)) ||
            !File.Exists(Path.Combine(projectPath, CompileSettings.JsonFileName))
        )
            throw new FileNotFoundException($"Project is corrupted or not created.\nPath to project settings: '{path}'.", path);
        
        var settingsPath = Path.Combine(projectPath, CompileSettings.JsonFileName);
        var dict =  JsonConvert.DeserializeObject<Dictionary<string, (object? @object, Type type)>>(File.ReadAllText(settingsPath));
        var compileSettings = new CompileSettings(settingsPath, dict);

        var proj = new Project(settings, compileSettings);
        

        var isDebug = (bool)compileSettings.Get(CompileSettings.DebugMode)!;
        Logger.Debug = isDebug;
        FileLogger.Debug = isDebug;
        
        return proj;
    }
}