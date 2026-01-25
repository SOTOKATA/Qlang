using System.Diagnostics;
using Core;
using Core.AST;
using ProjectManager.Project;
using ProjectManager.Settings;

namespace ProjectManager;

public class QLang
{
    private ProgramNode? _programNode;
    private List<string> _stringList = [];
    private List<double> _numberList = [];

    public bool Compile(string path, string? filename = null, bool useGZipCompress = false)
    {
        var code = File.ReadAllText(path);
        Compiler.Compiler c = new();

        _programNode = c.Compile(path, code);

        _stringList = c.StringList;
        _numberList = c.NumberList;
        
        var dirName = Path.GetDirectoryName(path);
        if (filename != null)
            path = dirName is null ? filename : Path.Combine(dirName, filename);
        
        SaveProgram(new QLIProgram
        {
            ProgramNode = _programNode,
            StringList = _stringList,
            NumberList = _numberList,
            ExternalLibraries = c.DllDependencies,
            SourceFileTable = c.SourceFileTable,
        }, path, useGZipCompress);

        return true;
    }

    private static void SaveDependencies(QLIProgram qliProgram, string path, string filename)
    {
        if (qliProgram.ExternalLibraries.Count == 0)
            return;
        
        var dirPath =  Path.Combine(path, filename + ".external.qli");
        var dirDepsPath = Path.Combine(dirPath, "dependents");
        
        if (Directory.Exists(dirPath))
            Directory.Delete(dirPath, true);
        
        Directory.CreateDirectory(dirPath);
        Directory.CreateDirectory(dirDepsPath);

        var externalLibs = qliProgram.ExternalLibraries;
        qliProgram.ExternalLibraries = [];

        foreach (var lib in externalLibs)
        {
            foreach (var depsFile in lib.DependenciesFilePaths)
                File.Copy(depsFile, Path.Combine(dirDepsPath, Path.GetFileName(depsFile)), true);
            foreach (var mainFile in lib.MainFilePaths)
                File.Copy(mainFile, Path.Combine(dirPath, Path.GetFileName(mainFile)), true);  
        }
    }

    private static void SaveProgram(QLIProgram qliProgram, string filePath, bool useGZipCompress)
    {
        var pathToFile = Path.GetDirectoryName(filePath);
        
        if (!Directory.Exists(Path.Combine(pathToFile ?? "", "build")))
            Directory.CreateDirectory(Path.Combine(pathToFile ?? "", "build"));

        SaveDependencies(qliProgram, Path.Combine(pathToFile ?? "", "build"), Path.GetFileNameWithoutExtension(filePath));
        
        var path = Path.Combine(pathToFile ?? "", "build", Path.GetFileNameWithoutExtension(filePath) + ".resource.qli");
        
        if (!File.Exists(path))
            File.Create(path).Close();
        
        File.Copy(Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), "qli" + OS.GetExecutableExtension()), Path.Combine(pathToFile ?? "", "build", Path.GetFileNameWithoutExtension(filePath) + OS.GetExecutableExtension()), true);
        
        if (useGZipCompress)
            File.WriteAllBytes(path, GZip.Compress(Json.Serialize(qliProgram)));
        else File.WriteAllText(path, Json.Serialize(qliProgram));
        
    }

    public void Run(List<string?>? args, string filename)
    {
        // TODO: Runtime execution (by build/program.exe)

        var exePath = Path.Combine("build", filename + OS.GetExecutableExtension());
        var resourcePath = Path.Combine("build", filename + ".resource.qli");

        if (!File.Exists(exePath) || !File.Exists(resourcePath))
            throw new ProjectException($"Files '{Path.GetFileName(exePath)}' and '{Path.GetFileName(resourcePath)}' is not found.\nProject is not compiled");

        Process.Start(new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/c \"{exePath}\" {args}",
        });

    }
}