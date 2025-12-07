namespace Qlang.Core.ProjectManager.Settings;

public class PluginSettings : Settings
{
    public PluginSettings(string path, Dictionary<string, object> dict) : base(path, dict)
    {
        Dictionary ??= new Dictionary<string, object?>
        {
            {
                "path", ""
            },
            {
                "name", ""
            },
            {
                "enabled", true
            },
        }; 
    }
}