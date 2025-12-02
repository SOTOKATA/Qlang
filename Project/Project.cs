using Newtonsoft.Json;

namespace Qlang.Project;

[JsonObject(MemberSerialization.OptOut)]
public class Project
{
    public string Path { get; }
    public string Name { get; }
    public string SettingsPath { get; }

    public string CompileFilePath { get; }
    
    private static JsonSerializerSettings _jsonSettings = new()
    {
        Formatting = Formatting.Indented,
        TypeNameHandling = TypeNameHandling.Auto,
        TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
    };
    
    public Project(string projectName, string projectPath, string compileFilePath)
    {
        Path = projectPath;
        Name = projectName;
        CompileFilePath = compileFilePath;
        SettingsPath = System.IO.Path.Combine(projectPath, $"{Name}.settings.json");
    }

    public void CreateProject()
    {
        if (!Directory.Exists(Path))
            Directory.CreateDirectory(Path);
        
        Save();
    }

    public void Save()
    {
        if (!File.Exists(SettingsPath))
            File.Create(SettingsPath).Close();
        
        string json = JsonConvert.SerializeObject(this, _jsonSettings);

        File.WriteAllText(SettingsPath, json);
    }

    public static Project? Load(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException("File is not found", path);

        string json = File.ReadAllText(path);

        return JsonConvert.DeserializeObject<Project>(json, _jsonSettings);
    }
}