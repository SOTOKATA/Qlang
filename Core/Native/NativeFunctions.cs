using System.Reflection;
using Core.Debug;
using Core.Exceptions;
using Core.NativeLib;
using Core.NativeLib.SystemLib;

namespace Core.Native;

public class NativeFunctionRegistry
{
    private readonly HashSet<NativeFunctionRegister> _functions = [];
    public List<IQlangLib> LoadedPlugins { get; set; } = [];

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
        if (string.IsNullOrWhiteSpace(lib.Namespace))
            throw new QlangCompileException($"Native function register exception: Namespace cannot be empty", -1, "NativeFunctions", "undefined file");
        
        if (string.IsNullOrWhiteSpace(lib.Class))
            throw new QlangCompileException($"Native function register exception: Class cannot be empty", -1, "NativeFunctions", "undefined file");
        
        if (string.IsNullOrWhiteSpace(lib.Name))
            throw new QlangCompileException($"Native function register exception: Name cannot be empty", -1, "NativeFunctions", "undefined file");
        
        foreach (var func in lib.GetFunctions())
            Register(new NativeFunctionRegister(lib.Namespace, lib.Class, func.name, func.body));
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