using System.Reflection;
using Qlang.Core.Lang.Dynamic.Exceptions;
using Qlang.Core.LangDebug;
using Qlang.NativeLib;
using Qlang.NativeLib.SystemLib;

namespace Qlang.Core.Lang.Interpreter.Native;

public class NativeFunctionRegistry
{
    private readonly HashSet<NativeFunctionRegister> _functions = [];
    private readonly List<IQlangLib> _loadedPlugins = [];

    public NativeFunctionRegistry()
    {
        RegisterLib(new ConsoleLib());
        RegisterLib(new ArrayLib());
        RegisterLib(new DateTimeLib());
        RegisterLib(new ExceptionLib());
        RegisterLib(new FileSystemLib());
        RegisterLib(new NumberLib());
        RegisterLib(new ObjectLib());
        RegisterLib(new ParserLib());
        RegisterLib(new RegexLib());
        RegisterLib(new StringLib());
        RegisterLib(new ConsoleCommandLib());
        RegisterLib(new MetaLib());
    }

    public void RegisterLib(IQlangLib lib)
    {
        if (lib.Namespace == "")
            throw new QlangCompileException($"Native function register exception: Namespace cannot be empty", -1, "NativeFunctions", "undefined file");
        
        if (lib.Class == "")
            throw new QlangCompileException($"Native function register exception: Class cannot be empty", -1, "NativeFunctions", "undefined file");
        
        if (lib.Name == "")
            throw new QlangCompileException($"Native function register exception: Name cannot be empty", -1, "NativeFunctions", "undefined file");
        
        foreach (var func in lib.GetFunctions())
            Register(new NativeFunctionRegister(lib.Namespace, lib.Class, func.name, func.body));
    }
    
    
    public void LoadNativeLib(string dllPath)
    {
        try
        {
            var assembly = Assembly.LoadFrom(dllPath);
        
            // Безопасная загрузка типов - игнорируем те, которые не загружаются
            Type[] types;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                // Берём только типы, которые успешно загрузились
                types = ex.Types.Where(t => t != null).ToArray()!;
            
                // Логируем предупреждения о пропущенных типах
                foreach (var loaderException in ex.LoaderExceptions.Where(e => e != null).Distinct())
                {
                    Logger.Warn($"Could not load some types from {Path.GetFileName(dllPath)}: {loaderException!.Message}", "PluginLoader");
                }
            }
        
            var libTypes = types
                .Where(t => typeof(IQlangLib).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false });
        
            var foundPlugin = false;
            foreach (var type in libTypes)
            {
                var nativeLib = (IQlangLib)Activator.CreateInstance(type)!;
                RegisterLib(nativeLib);
                _loadedPlugins.Add(nativeLib);
                foundPlugin = true;
            
                Logger.Log($"Loaded native lib: {nativeLib.Name} v{nativeLib.Version}", "NativeLibLoader");
            }
        
            if (!foundPlugin)
            {
                Logger.Warn($"No IQlangLib implementations found in {Path.GetFileName(dllPath)}", "NativeLibLoader");
            }
        }
        catch (Exception ex)
        {
            throw new QlangCompileException($"Failed to load native lib from '{dllPath}': {ex.Message}", -1,  "PluginLoader", dllPath);
        }
    }
    
    private void Register(NativeFunctionRegister nativeRegister)
    {
        if (!_functions.Add(nativeRegister))
            throw new QlangCompileException($"Native function register exception: Duplicate function: '{nativeRegister.GetName()}'", -1, "NativeFunctions", "undefined file");
    }
    
    public object? Call(string name, object?[]? args)
    {
        var func = _functions.FirstOrDefault(x => x.GetName() == name);
        
        if (func == null)
            throw new Exception($"Function '{name}' not found");

        Logger.Log($"name='{name}'", "NativeCall");
        if (args is not null)
        {
            var arguments = func.Function.Method.GetParameters();

            string debug = "args=\n";
            for (var index = 0; index < arguments.Length; index++)
            {
                var type = arguments[index];

                debug += $"{index}: type={type.ParameterType} ";

                if (type.ParameterType == typeof(int))
                    args[index] = int.Parse(args[index]?.ToString());
                else if (type.ParameterType == typeof(double))
                    args[index] = args[index]!.ToString().ParseNumber();
            }

            debug += "\nSent:\n";

            for (var index = 0; index < args.Length; index++)
            {
                var arg = args[index];
                debug += $"{index}: '{arg}' type='{(arg is null ? "NULL" : arg.GetType().Name)}' ";
            }


            Logger.Log(debug + "\nArgs count: " + args.Length, "NativeCall::Arguments");
        }
        else
            Logger.Log("args='null' (null)", "NativeCall");

        try
        {
            return func.Function.DynamicInvoke(args);
        }
        // Remove Delegate Exception
        catch (TargetInvocationException ex)
        {
            if (ex.InnerException != null)
                throw ex.InnerException;
            throw;
        }
    }
}