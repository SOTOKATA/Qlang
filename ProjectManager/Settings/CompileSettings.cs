namespace ProjectManager.Settings;

public class CompileSettings : Settings
{
    public CompileSettings(string path, Dictionary<string, (object? @object, Type type)>? dict) : base(path, dict)
    {
        Dictionary ??= new Dictionary<string, (object? @object, Type type)>
        {
            { OutputFilename, ("program", typeof(string)) },
            { DebugMode, (false, typeof(bool)) },
        };
    }

    public static string OutputFilename => "output_filename";
    public static string DebugMode => "debug_mode";

    public static string JsonFileName => "compile.settings.json";
}