using System.Reflection;
using System.Text.RegularExpressions;
using Qlang.Core.Lang.Dynamic;
using Qlang.Core.Lang.Dynamic.Exceptions;
using Qlang.Core.LangDebug;
using Qlang.NativeLib;

namespace Qlang.Core.Lang.Interpreter.Native;

public class NativeFunctionRegistry
{
    private readonly Dictionary<string, Delegate> _functions = new();
    private readonly List<IQlangLib> _loadedPlugins = [];
    
    public NativeFunctionRegistry()
    {
        Register("lib.to_string_number", (Func<double, string, string>)((o, pattern) => o.ToString(pattern)));
      
        // Console
        Register("lib.cmd_write", (Action<string?>)Console.Write);
        Register("lib.cmd_width", (Func<double>)(() => Console.WindowWidth));
        Register("lib.cmd_height", (Func<double>)(() => Console.WindowHeight));
        Register("lib.cmd_cursor_visible", (Action<bool>)(isVisible => Console.CursorVisible = isVisible));
        Register("lib.cmd_read", Console.ReadLine);
        Register("lib.cmd_key", (Func<bool, string>)(intercept => Console.ReadKey(intercept).KeyChar.ToString()));
        Register("lib.cmd_key_available", (Func<bool>)(() => Console.KeyAvailable));
        Register("lib.cmd_clear", (Action)Console.Clear);
        Register("lib.cmd_fcolor", (Action<string>)(color => NativeConsole.SetForegroundColor((color))));
        Register("lib.cmd_bcolor", (Action<string>)(color => NativeConsole.SetBackgroundColor((color))));
        Register("lib.cmd_reset_colors", Console.ResetColor);
        Register("lib.cmd_cursor_position", (Action<int, int>)(Console.SetCursorPosition));
        // Number
        Register("lib.try_parse_number", (Func<object, bool>)(number => number.ToString().TryParseNumber(out _)));
        Register("lib.random", (Func<int, int, int>)((num1, num2) => new Random().Next(num1, num2)));
        // Exception
        Register("lib.exception", (Action<string>)(msg => throw new QlangRuntimeException(msg, null)));
        
        // String
        Register("lib.str_at", (Func<string, int, string>)((str, index) => str[index].ToString()));
        Register("lib.str_length", (Func<string, int>)(NativeString.GetLength));
        Register("lib.str_is_primitive", (Func<object?, bool>)(str => str is string));
        Register("lib.str_is_str", (Func<object?, bool>)(str => str is DynamicClass { ClassName: "String" }));
        Register("lib.str_null_or_empty", (Func<string, bool>)(string.IsNullOrEmpty));
        Register("lib.str_null_or_white_space", (Func<string, bool>)(string.IsNullOrWhiteSpace));
        Register("lib.str_trim", (Func<string, string>)(msg => msg.Trim()));
        Register("lib.str_trim_start", (Func<string, string>)(msg => msg.TrimStart()));
        Register("lib.str_trim_end", (Func<string, string>)(msg => msg.TrimEnd()));
        Register("lib.str_sub_string", (Func<string, int, int, string>)((msg, start, length) => msg.Substring(start, length)));
        Register("lib.str_to_lower", (Func<string, string>)((str) => str.ToLower()));
        Register("lib.str_to_upper", (Func<string, string>)((str) => str.ToUpper()));
        Register("lib.str_split", (Func<string, string, List<object>>)((str, splitPattern) => str.Split(splitPattern).Cast<object>().ToList()));
        Register("lib.str_join", (Func<List<object?>, string, string>)((arr, joinPattern) => string.Join(joinPattern, arr)));
        // Register("lib.str_length", (Action<string>)(msg => msg.Length);
        
        // Time
        Register("lib.time_wait", (Action<int>)(Thread.Sleep));
        
        // DateTime
        Register("lib.datetime_now", (Func<DateTime>)(() => DateTime.Now));
        
        // Array
        Register("lib.list_create", (Func<List<object>>)(() => []));
        Register("lib.list_insert", (Action<List<object>, int, object?>)((list, pos, item) => list.Insert(pos, item)));
        Register("lib.list_add", (Action<List<object>, object>)((list, item) => list.Add(item)));
        Register("lib.list_get", (Func<List<object>, int, object?>)((list, idx) => list[idx]));
        Register("lib.list_is", (Func<object?, bool>)(obj => obj is List<object>));
        Register("lib.list_is_array", (Func<object?, bool>)(obj => obj is DynamicClass { ClassName: "Array" }));
        Register("lib.list_set", (Action<List<object>, int, object>)((list, idx, val) => list[idx] = val));
        Register("lib.list_count", (Func<List<object>, int>)(list => list.Count));
        Register("lib.list_clear", (Action<List<object>>)(list => list.Clear()));
        Register("lib.list_contains", (Func<List<object>, object, bool>)((list, item) => list.Contains(item)));
        Register("lib.list_remove_at", (Action<List<object>, int>)((list, idx) => list.RemoveAt(idx)));
        Register("lib.list_index_of", (Func<List<object>, object, int>)((list, item) => list.IndexOf(item)));
        
        // Parser
        Register("lib.parse_int", (Func<object?, int>)(obj => (int)NativeParser.Parse(obj, "int")));
        Register("lib.parse_float", (Func<object, double>)(obj => (double)NativeParser.Parse(obj, "float")));
        Register("lib.parse_number", (Func<object, double>)(obj => (double)NativeParser.Parse(obj, "double")));
        Register("lib.parse_string", (Func<object, string>)(obj => (string)NativeParser.Parse(obj, "string")));
        
        // Object
        Register("lib.obj_is_null",  (Func<object?, bool>)(obj => obj is null));
        
        // Regex
        Register("lib.regex_replace", (Func<string?, string, string, string?>)(Regex.Replace));
        
        // File
        Register("lib.file_exists", (Func<string, bool>)(File.Exists));
        Register("lib.file_set_content", (Action<string, string>)(File.WriteAllText));
        Register("lib.file_append_content", (Action<string, string>)(File.AppendAllText));
        Register("lib.file_get_content", (Func<string, string>)(File.ReadAllText));
        Register("lib.file_create", (Action<string>)((path) => File.Create(path).Close()));
        Register("lib.file_remove", (Action<string>)(File.Delete));
        
        // Path
        Register("lib.path_combine", (Func<List<object>, string>)((arr) => Path.Combine(arr.Cast<string>().ToArray())));
        Register("lib.path_extension", (Func<string, string?>)Path.GetExtension);
        Register("lib.path_has_extension", (Func<string, bool>)Path.HasExtension);
        Register("lib.path_change_extension", (Func<string?, string?, string?>)Path.ChangeExtension);
        Register("lib.path_file_name_without_extension", (Func<string, string?>)Path.GetFileNameWithoutExtension);
        Register("lib.path_file_name", (Func<string, string?>)Path.GetFileName);
        Register("lib.path_exists", (Func<string, bool>)Path.Exists);

        // Directory
        Register("lib.directory_exists", (Func<string, bool>)Directory.Exists);
        Register("lib.directory_create", (Action<string>)((path) => Directory.CreateDirectory(path)));
        Register("lib.directory_remove", (Action<string, bool>)(Directory.Delete));
    }

    public void RegisterPublic(string nativeName, string name, Delegate handler)
    {
        Register(nativeName + "." + name, handler);
    }

    // public void LoadPlugin(string dllPath)
    // {
    //     try
    //     {
    //         var assembly = Assembly.LoadFrom(dllPath);
    //         var pluginTypes = assembly.GetTypes()
    //             .Where(t => typeof(IQlangLib).IsAssignableFrom(t) && !t.IsInterface);
    //         
    //         foreach (var type in pluginTypes)
    //         {
    //             var plugin = (IQlangLib)Activator.CreateInstance(type)!;
    //             plugin.Register(this);
    //             _loadedPlugins.Add(plugin);
    //             
    //             Logger.Log($"Loaded plugin: {plugin.Name} v{plugin.Version}", "PluginLoader");
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         throw new Exception($"Failed to load plugin from '{dllPath}': {ex.Message}", ex);
    //     }
    // }
    
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
                nativeLib.Register(this);
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
            throw new Exception($"Failed to load native lib from '{dllPath}': {ex.Message}", ex);
        }
    }
    
    private void Register(string name, Delegate handler)
    {
        if (!_functions.TryAdd(name, handler))
            throw new QlangCompileException($"Native function register exception: Duplicate function name: {name}", -1, "NativeFunctions", "undefined file");
    }
    
    public object? Call(string name, object?[]? args)
    {
        if (!_functions.TryGetValue(name, out var func))
            throw new Exception($"Function '{name}' not found");

        Logger.Log($"name='{name}'", "NativeCall");
        if (args is not null)
        {
            var arguments = func.Method.GetParameters();

            string debug = "args=\n";
            for (var index = 0; index < arguments.Length; index++)
            {
                var type = arguments[index];

                debug += $"{index}: type={type.ParameterType} ";

                if (type.ParameterType == typeof(int))
                    args[index] = int.Parse(args[index].ToString());
                else if (type.ParameterType == typeof(double))
                    args[index] = args[index].ToString().ParseNumber();
                // else if (type.ParameterType == typeof(string) && args[index] is DynamicClass { ClassName: "String" })
                // {
                //     var value = args[index] as DynamicClass;
                //     
                //     if (value?.Body.FirstOrDefault(x => x is AssignmentNode { IsPrivate: true, VariableName: "_value" }) is AssignmentNode var)
                //         args[index] = var.Value;
                // }
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
            return func.DynamicInvoke(args);
        }
        // Remove Delegate Exception
        catch (System.Reflection.TargetInvocationException ex)
        {
            if (ex.InnerException != null)
                throw ex.InnerException;
            throw;
        }
    }
}