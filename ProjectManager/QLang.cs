using System.Diagnostics;
using Core;
using Core.AST;
using ProjectManager.Project;

namespace ProjectManager;

public class QLang
{
    public bool Compile(string path, string? filename = null, bool isPublish = false)
    {
        var code = File.ReadAllText(path);
        Compiler.Compiler c = new();

        var programNode = c.Compile(path, code, isPublish);
        
        var dirName = Path.GetDirectoryName(path);
        if (filename != null)
            path = dirName is null ? filename : Path.Combine(dirName, filename);
        
        SaveProgram(new QLIProgram
        {
            ProgramNode = programNode,
            NumberList = c.NumberList,
            ExternalLibraries = c.DllDependencies,
            StringPoolTable =  c.StringPoolTable,
        }, new QLIDebug
        {
            SourceFileTable = c.SourceFileTable,
            DebugTable = c.DebugTable
        }, path, isPublish);

        return true;
    }

    private static void SaveDependencies(QLIProgram qliProgram, string path, string filename)
    {
        if (qliProgram.ExternalLibraries.Count == 0)
            return;
        
        var dirPath =  Path.Combine(path, filename + ".external.qli");
        
        if (Directory.Exists(dirPath))
            Directory.Delete(dirPath, true);
        
        Directory.CreateDirectory(dirPath);

        var externalLibs = qliProgram.ExternalLibraries;
        qliProgram.ExternalLibraries = [];

        foreach (var lib in externalLibs)
        {
            var currentLibPath = Path.Combine(dirPath, lib.Name);
            var currentLibDepsPath =  Path.Combine(currentLibPath, "deps");
            
            Directory.CreateDirectory(currentLibPath);
            Directory.CreateDirectory(currentLibDepsPath);
            
            foreach (var depsFile in lib.DependenciesFilePaths)
                File.Copy(depsFile, Path.Combine(currentLibDepsPath, Path.GetFileName(depsFile)), true);
            foreach (var mainFile in lib.MainFilePaths)
                File.Copy(mainFile, Path.Combine(currentLibPath, Path.GetFileName(mainFile)), true);  
        }
    }

    private static void SaveProgram(QLIProgram qliProgram, QLIDebug qliDebug, string filePath, bool isPublish)
    {
        var pathToFile = Path.GetDirectoryName(filePath);
        
        if (!Directory.Exists(Path.Combine(pathToFile ?? "", "build")))
            Directory.CreateDirectory(Path.Combine(pathToFile ?? "", "build"));

        SaveDependencies(qliProgram, Path.Combine(pathToFile ?? "", "build"), Path.GetFileNameWithoutExtension(filePath));
        
        var basePath = Path.Combine(pathToFile ?? "", "build", Path.GetFileNameWithoutExtension(filePath));
        var path = basePath + ".resource.qli";
        var debugPath = basePath + ".debug.qli";
        
        if (!File.Exists(path))
            File.Create(path).Close();

        if (!File.Exists(debugPath) && !isPublish)
            File.Create(debugPath).Close();
        
        File.Copy(Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), "qli" + OS.GetExecutableExtension()), Path.Combine(pathToFile ?? "", "build", Path.GetFileNameWithoutExtension(filePath) + OS.GetExecutableExtension()), true);
        
        File.WriteAllBytes(path, Brotli.Compress(Core.MessagePack.Serialize(qliProgram)));
        
        if (!isPublish)
            File.WriteAllBytes(debugPath, Brotli.Compress(Core.MessagePack.Serialize(qliDebug)));
    }

    // public void Run(List<string?>? args, string filename)
    // {
    //     // TODO: Runtime execution (by build/program.exe)
    //
    //     var exePath = Path.Combine("build", filename + OS.GetExecutableExtension());
    //     var resourcePath = Path.Combine("build", filename + ".resource.qli");
    //
    //     if (!File.Exists(exePath) || !File.Exists(resourcePath))
    //         throw new ProjectException($"Files '{Path.GetFileName(exePath)}' and '{Path.GetFileName(resourcePath)}' is not found.\nProject is not compiled");
    //
    //     Process.Start(new ProcessStartInfo
    //     {
    //         FileName = "cmd.exe",
    //         Arguments = $"/c \"{exePath}\" {args}",
    //     });
    //
    // }
    
    public void Run(List<string?>? args, string filename)
    {
        var exePath = Path.Combine("build", filename + OS.GetExecutableExtension());
        var resourcePath = Path.Combine("build", filename + ".resource.qli");

        if (!File.Exists(exePath) || !File.Exists(resourcePath))
            throw new ProjectException(
                $"Files '{Path.GetFileName(exePath)}' and '{Path.GetFileName(resourcePath)}' is not found.\nProject is not compiled");

        var startInfo = new ProcessStartInfo
        {
            FileName = exePath,
            UseShellExecute = false
        };

        if (args != null)
            foreach (var arg in args.OfType<string>())
                startInfo.ArgumentList.Add(arg);

        var proc = new Process();
        
        proc.StartInfo = startInfo;
        
        proc.Start();
        proc.WaitForExit();
    }

}