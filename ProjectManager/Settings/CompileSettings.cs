namespace ProjectManager.Settings;

public class CompileSettings : Settings
{
    public CompileSettings(string path, Dictionary<string, SettingsItem>? dict) : base(path, dict)
    {
        Dictionary ??= new Dictionary<string, SettingsItem>
        {
            { OutputFilename, new("program", typeof(string)) },
            { DebugMode, new(false, typeof(bool)) },
            { GZipCompress, new(true, typeof(bool)) },
            { JsonIndented, new(false, typeof(bool)) },
        };
    }

    public static string OutputFilename => "output_filename";
    public static string DebugMode => "debug_mode";
    public static string GZipCompress => "gzip_compress";
    public static string JsonIndented => "json_indented";

    public static string JsonFileName => "compile.settings.json";
}