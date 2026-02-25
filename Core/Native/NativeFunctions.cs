using System.Reflection;
using Core.Exceptions;
using Core.NativeLib;
using Core.NativeLib.SystemLib;

namespace Core.Native;

public class NativeFunctionRegistry
{
    private readonly HashSet<NativeFunctionRegister> _functions = [];

    public NativeFunctionRegistry()
    {
        RegisterLib(new SystemLib());
    }

    public void RegisterLib(IQlangLib lib)
    {
        if (string.IsNullOrWhiteSpace(lib.Name))
            throw new QlangCompileException($"Native function register exception: Name cannot be empty", -1, "NativeFunctions", "undefined file");
        
        if (lib.Namespaces.Any(@namespace => string.IsNullOrWhiteSpace(@namespace.Name)))
            throw new QlangCompileException($"Native function register exception: Namespace cannot be empty (lib: '{lib.Name}')", -1, "NativeFunctions", "undefined file");
        
        if (lib.Namespaces.Any(@namespace => @namespace.Classes.Any(@class => string.IsNullOrWhiteSpace(@class.Name))))
            throw new QlangCompileException($"Native function register exception: Class cannot be empty (lib: '{lib.Name}')", -1, "NativeFunctions", "undefined file");
        
        foreach (var libNamespace in lib.Namespaces)
            foreach (var libClass in libNamespace.Classes)
                foreach (var func in libClass.GetFunctions())
                    Register(new NativeFunctionRegister(libNamespace.Name, libClass.Name, func.name, func.body));
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
            throw new Exception($"Native function '{name}' not found");

        if (args is not null)
        {
            var arguments = func.Function.Method.GetParameters();

            for (var index = 0; index < arguments.Length; index++)
            {
                var type = arguments[index];

                if (type.ParameterType == typeof(int))
                    args[index] = int.Parse(args[index]?.ToString());
                else if (type.ParameterType == typeof(double))
                    args[index] = args[index]!.ToString().ParseNumber();
            }
        }

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