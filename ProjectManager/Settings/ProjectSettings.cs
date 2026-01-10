namespace ProjectManager.Settings;

public class ProjectSettings : Settings
{
    public ProjectSettings(string path, Dictionary<string, (object? @object, Type type)>? dict) : base(path, dict)
    {
        Dictionary ??= new Dictionary<string, (object? @object, Type type)>
        {
            { RootPath, ("", typeof(string)) },
            { ProjectName, ("", typeof(string)) },
            { MainFilePath, ("", typeof(string)) },
            { BuildDirectoryPath, ("build", typeof(string)) }
        };
    }

    public static string RootPath => "root_path";
    public static string ProjectName => "project_name";
    public static string MainFilePath => "main_file_path";
    public static string BuildDirectoryPath => "build_directory_path";

    public static string JsonFileName => "project.settings.json";
}