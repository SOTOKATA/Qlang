namespace ProjectManager.Settings;

public class CompileSettings : Settings
{
    public CompileSettings(string path, Dictionary<string, object?>? dict) : base(path, dict)
    {
        Dictionary ??= new Dictionary<string, object?>
        {
            { "output_filename", "program" },
            { "debug_mode", false },
        };
    }
}