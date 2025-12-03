using Newtonsoft.Json;
using Qlang.Project.Settings;

namespace Qlang.Project;

public partial class Project
{
    private static ProjectSettings _settings;
    
    private static CompileSettings _compileSettings;

    private readonly QLang _qlang;
    
    public Project(string projectName, string projectPath, string mainFilePath)
    {
        _settings = new ProjectSettings(Path.Combine(projectPath, "project.settings.json"), null);
        
        _settings.Set("path", projectPath);
        _settings.Set("name", projectName);
        _settings.Set("main_file_path", mainFilePath);
        
        // create main.ql
        if (!File.Exists(mainFilePath))
        {
            File.WriteAllText(mainFilePath, """
                                            include "$lib/base"
                                            
                                            function main(): {
                                                Console.println("Hello World!");
                                            }
                                            """);
        }
        
        SaveProject();

        string settingsPath = Path.Combine(projectPath, "compile.settings.json");
        if (!File.Exists(settingsPath))
            _compileSettings = new CompileSettings(settingsPath, null);
        else
        {
            var dict =  JsonConvert.DeserializeObject<Dictionary<string, object?>>(File.ReadAllText(settingsPath));
            _compileSettings = new CompileSettings(settingsPath, dict);
        }
        
        _compileSettings.Save();
        
        _qlang = new QLang();
    }

    public void SaveProject()
    {
        if (!File.Exists(_settings.GetPath()))
            File.Create(_settings.GetPath()).Close();

        _settings.Save();
    }

    // path to settings file
    public static Project LoadProject(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"File '{path}' is not found.\nThe project may be corrupted or not created.", path);

        var settings = new ProjectSettings(path, Settings.Settings.LoadDictionary(path));
        
        return new Project(settings.GetString("name"), settings.GetString("path"), settings.GetString("main_file_path"));
    }
}