namespace ProjectManager.Project;

public partial class Project
{
    public static void SetCompileSetting(string param, object value)
    {
        try
        {
            _compileSettings.Set(param, value);
        }
        catch (Exception ex)
        {
            ExceptionManager.ThrowMessage(ex.Message);
            return;
        }

        ConsoleLogger.Set($"{param}: {value}");
        _compileSettings.Save();
    }

    public static object? GetCompileSetting(string param)
    {
        return _compileSettings?.Get(param);
    }

    public void Compile(string? filename = null)
    {
        _qlang.Compile(Path.Combine(Settings.GetString("path"), Settings.GetString("main_file_path")), filename);
    }

    public void Run(List<string?>? args)
    {
        if (_qlang.Compile(Path.Combine(Settings.GetString("path"), Settings.GetString("main_file_path"))))
            _qlang.Run(args);
    }
}