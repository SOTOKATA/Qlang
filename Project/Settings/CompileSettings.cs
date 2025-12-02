namespace Qlang.Project.Settings;

public class CompileSettings : Settings
{
    public CompileSettings(string path, Dictionary<string, object?>? dict) : base(path, dict)
    {
        Dictionary ??= new Dictionary<string, object?>
        {
            { "debug", false },
            { "name", "null" },
            { "version", "0.0.1" },
            { "author", "null" }
        };
    }
}