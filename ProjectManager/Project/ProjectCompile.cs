using System.Diagnostics;
using ProjectManager.Settings;

namespace ProjectManager.Project;

public partial class Project
{
    public void SetCompileSetting(string param, object value)
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

    public object? GetCompileSetting(string param)
    {
        return _compileSettings.Get(param);
    }

    public bool Compile(bool isPublish)
    {
        var rootPath = Path.Combine(_settings.GetString(ProjectSettings.RootPath),
            _settings.GetString(ProjectSettings.MainFilePath));

        var outputName = _compileSettings.GetString(CompileSettings.OutputFilename);
        
        return _qlang.Compile(rootPath, outputName, isPublish);
    }

    public void Run(List<string?>? args)
    {
        if (Compile(false))
            _qlang.Run(args, _compileSettings.GetString(CompileSettings.OutputFilename));
    }
}