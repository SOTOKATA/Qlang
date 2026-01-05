namespace ProjectManager.Project;

public partial class Project
{
    public static void SetCompileSetting(string param, object value)
    {
        try
        {
            CompileSettings.Set(param, value);
        }
        catch (Exception ex)
        {
            ExceptionManager.ThrowMessage(ex.Message);
            return;
        }

        ConsoleLogger.Set($"{param}: {value}");
        CompileSettings.Save();
    }

    public static object? GetCompileSetting(string param)
    {
        return CompileSettings?.Get(param);
    }

    public void Compile()
    {
        _qlang.Compile(Path.Combine(Settings.GetString("path"), Settings.GetString("main_file_path")), CompileSettings.GetString("filename"));
    }

    public void Run(List<string?>? args)
    {
        if (_qlang.Compile(Path.Combine(Settings.GetString("path"), Settings.GetString("main_file_path")), CompileSettings.GetString("filename")))
            _qlang.Run(args);
    }
}