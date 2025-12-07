namespace Qlang.Core.ProjectManager.Settings;

public class PluginsSettings : Settings
{
    public PluginsSettings(string path, Dictionary<string, object?>? dict) : base(path, dict)
    {
        Dictionary ??= new Dictionary<string, object?>()
        {
            { "plugins", new List<PluginSettings>() }
        };
    }
}