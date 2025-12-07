using Qlang.Core.Lang.Dynamic.Exceptions;

namespace Qlang.Core.ProjectManager.Project;

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
        return CompileSettings.Get(param);
    }

    public void Compile()
    {
        // _qlang.Compile(_settings.GetString("main_file_path"));
    }

    public void Run()
    {
        if (_qlang.Compile(Path.Combine(Settings.GetString("path"), Settings.GetString("main_file_path"))))
            _qlang.Run();
    }
}