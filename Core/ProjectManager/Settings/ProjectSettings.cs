namespace Qlang.Core.ProjectManager.Settings;

public class ProjectSettings : Settings
{
    public ProjectSettings(string path, Dictionary<string, object?>? dict) : base(path, dict)
    {
        Dictionary ??= new Dictionary<string, object?>
        {
            { "path", "" },
            { "name", "" },
            { "main_file_path", "" }
        };
    }
}