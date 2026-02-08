namespace ProjectManager.Settings;

public class CompileSettings : Settings
{
    public CompileSettings(string path, Dictionary<string, SettingsItem>? dict) : base(path, dict)
    {
        Dictionary ??= new Dictionary<string, SettingsItem>
        {
            { OutputFilename, new("program", typeof(string)) },
            { SaveAlsoASTVersion, new(false, typeof(bool)) },
        };
    }

    public static string OutputFilename => "output_filename";
    public static string SaveAlsoASTVersion => "save_also_ast_version";

    public static string JsonFileName => "compile.settings.json";
}