namespace ProjectManager.Settings;

public class ProjectSettings : Settings
{
    public ProjectSettings(string path, Dictionary<string, SettingsItem>? dict) : base(path, dict)
    {
        Dictionary ??= new Dictionary<string, SettingsItem>
        {
            { RootPath, new("", typeof(string)) },
            { ProjectName, new("", typeof(string)) },
            { MainFilePath, new("", typeof(string)) }
        };
    }

    public static string RootPath => "root_path";
    public static string ProjectName => "project_name";
    public static string MainFilePath => "main_file_path";
    public static string JsonFileName => "project.settings.json";
}