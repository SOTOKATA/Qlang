using System.Reflection;
using System.Text.RegularExpressions;
using Qlang.Core.Lang.AST;
using Qlang.Core.Lang.Dynamic;
using Qlang.Core.Lang.Dynamic.Exceptions;
using Qlang.Core.LangDebug;
using Qlang.Core.ProjectManager;
using Qlang.Plugins;

namespace Qlang.Core.Lang.Interpreter.Native;

public class NativeFunctionRegistry
{
    private readonly Dictionary<string, Delegate> _functions = new();
    private readonly List<IQlangPlugin> _loadedPlugins = new();
    
    public NativeFunctionRegistry()
    {
        Register("to_string_number", (Func<double, string, string>)((o, pattern) => o.ToString(pattern)));
      
        // Console
        Register("cmd_write", (Action<string?>)Console.Write);
        Register("cmd_cursor_visible", (Action<bool>)(isVisible => Console.CursorVisible = isVisible));
        Register("cmd_read", Console.ReadLine);
        Register("cmd_key", (Func<bool, string>)(intercept => Console.ReadKey(intercept).KeyChar.ToString()));
        Register("cmd_key_available", (Func<bool>)(() => Console.KeyAvailable));
        Register("cmd_clear", (Action)Console.Clear);
        Register("cmd_fcolor", (Action<string>)(color => NativeConsole.SetForegroundColor((color))));
        Register("cmd_bcolor", (Action<string>)(color => NativeConsole.SetBackgroundColor((color))));
        Register("cmd_reset_colors", Console.ResetColor);
        Register("cmd_cursor_position", (Action<int, int>)(Console.SetCursorPosition));
        // Number
        Register("try_parse_number", (Func<object, bool>)(number => number.ToString().TryParseNumber(out _)));
        Register("random", (Func<int, int, int>)((num1, num2) => new Random().Next(num1, num2)));
        // Exception
        Register("exception", (Action<string>)(msg => throw new QlangRuntimeException(msg, null, null)));
        
        // String
        Register("str_length", (Func<string, int>)(NativeString.GetLength));
        Register("str_is_primitive", (Func<object?, bool>)(str => str is string));
        Register("str_is_str", (Func<object?, bool>)(str => str is DynamicClass { ClassName: "String" }));
        Register("str_null_or_empty", (Func<string, bool>)(string.IsNullOrEmpty));
        Register("str_null_or_white_space", (Func<string, bool>)(string.IsNullOrWhiteSpace));
        Register("str_trim", (Func<string, string>)(msg => msg.Trim()));
        Register("str_trim_start", (Func<string, string>)(msg => msg.TrimStart()));
        Register("str_trim_end", (Func<string, string>)(msg => msg.TrimEnd()));
        Register("str_sub_string", (Func<string, int, int, string>)((msg, start, length) => msg.Substring(start, length)));
        Register("str_to_lower", (Func<string, string>)((str) => str.ToLower()));
        Register("str_to_upper", (Func<string, string>)((str) => str.ToUpper()));
        Register("str_split", (Func<string, string, List<object>>)((str, splitPattern) => str.Split(splitPattern).Cast<object>().ToList()));
        Register("str_join", (Func<List<object?>, string, string>)((arr, joinPattern) => string.Join(joinPattern, arr)));
        // Register("str_length", (Action<string>)(msg => msg.Length);
        
        // Time
        Register("time_wait", (Action<int>)(Thread.Sleep));
        
        // DateTime
        Register("datetime_now", (Func<DateTime>)(() => DateTime.Now));
        
        // Array
        Register("list_create", (Func<List<object>>)(() => []));
        Register("list_insert", (Action<List<object>, int, object?>)((list, pos, item) => list.Insert(pos, item)));
        Register("list_add", (Action<List<object>, object>)((list, item) => list.Add(item)));
        Register("list_get", (Func<List<object>, int, object?>)((list, idx) => list[idx]));
        Register("list_is", (Func<object?, bool>)(obj => obj is List<object>));
        Register("list_is_array", (Func<object?, bool>)(obj => obj is DynamicClass { ClassName: "Array" }));
        Register("list_set", (Action<List<object>, int, object>)((list, idx, val) => list[idx] = val));
        Register("list_count", (Func<List<object>, int>)(list => list.Count));
        Register("list_clear", (Action<List<object>>)(list => list.Clear()));
        Register("list_contains", (Func<List<object>, object, bool>)((list, item) => list.Contains(item)));
        Register("list_remove_at", (Action<List<object>, int>)((list, idx) => list.RemoveAt(idx)));
        Register("list_index_of", (Func<List<object>, object, int>)((list, item) => list.IndexOf(item)));
        
        // Parser
        Register("parse_int", (Func<object?, int>)(obj => (int)NativeParser.Parse(obj, "int")));
        Register("parse_float", (Func<object, double>)(obj => (double)NativeParser.Parse(obj, "float")));
        Register("parse_number", (Func<object, double>)(obj => (double)NativeParser.Parse(obj, "double")));
        Register("parse_string", (Func<object, string>)(obj => (string)NativeParser.Parse(obj, "string")));
        
        // Object
        Register("obj_is_null",  (Func<object?, bool>)(obj => obj is null));
        
        // Regex
        Register("regex_replace", (Func<string?, string, string, string?>)(Regex.Replace));
        
        // File
        Register("file_exists", (Func<string, bool>)(File.Exists));
        Register("file_set_content", (Action<string, string>)(File.WriteAllText));
        Register("file_append_content", (Action<string, string>)(File.AppendAllText));
        Register("file_get_content", (Func<string, string>)(File.ReadAllText));
        Register("file_create", (Action<string>)((path) => File.Create(path).Close()));
        Register("file_remove", (Action<string>)(File.Delete));
        
        // Path
        Register("path_combine", (Func<List<object>, string>)((arr) => Path.Combine(arr.Cast<string>().ToArray())));
        Register("path_extension", (Func<string, string?>)Path.GetExtension);
        Register("path_has_extension", (Func<string, bool>)Path.HasExtension);
        Register("path_change_extension", (Func<string?, string?, string?>)Path.ChangeExtension);
        Register("path_file_name_without_extension", (Func<string, string?>)Path.GetFileNameWithoutExtension);
        Register("path_file_name", (Func<string, string?>)Path.GetFileName);
        Register("path_exists", (Func<string, bool>)Path.Exists);

        // Directory
        Register("directory_exists", (Func<string, bool>)Directory.Exists);
        Register("directory_create", (Action<string>)((path) => Directory.CreateDirectory(path)));
        Register("directory_remove", (Action<string, bool>)(Directory.Delete));
    }

    public void RegisterPublic(string name, Delegate handler)
    {
        Register(name, handler);
    }
    
    public void LoadPlugin(string dllPath)
    {
        try
        {
            var assembly = Assembly.LoadFrom(dllPath);
            var pluginTypes = assembly.GetTypes()
                .Where(t => typeof(IQlangPlugin).IsAssignableFrom(t) && !t.IsInterface);
            
            foreach (var type in pluginTypes)
            {
                var plugin = (IQlangPlugin)Activator.CreateInstance(type)!;
                plugin.Register(this);
                _loadedPlugins.Add(plugin);
                
                Logger.Log($"Loaded plugin: {plugin.Name} v{plugin.Version}", "PluginLoader");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to load plugin from '{dllPath}': {ex.Message}", ex);
        }
    }
    
    public void LoadPluginsFromDirectory(string directory)
    {
        if (!Directory.Exists(directory))
        {
            Logger.Log($"Plugin directory not found: {directory}", "PluginLoader");
            return;
        }
        
        var dllFiles = Directory.GetFiles(directory, "*.dll");
        foreach (var dll in dllFiles)
        {
            try
            {
                LoadPlugin(dll);
            }
            catch (Exception ex)
            {
                Logger.Log($"Error loading {dll}: {ex.Message}", "PluginLoader");
            }
        }
    }
    
    private void Register(string name, Delegate handler)
    {
        _functions[name] = handler;
    }
    
    public object? Call(string name, object?[]? args)
    {
        if (!_functions.TryGetValue(name, out var func))
            throw new Exception($"Function '{name}' not found");

        // foreach (var var in args)
        // {
        //     if (var is DynamicClass d)
        //     {
        //         ConsoleLogger.Info($"ClassInfo: name='{d.Name}', className='{d.ClassName}'");
        //     } else ConsoleLogger.Info($"ArgInfo: '{var}' with type '{var?.GetType()?.Name}'");
        // }

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
                else if (type.ParameterType == typeof(string) && args[index] is DynamicClass { ClassName: "String" })
                {
                    ConsoleLogger.Info("Is Class.String");
                    var value = args[index] as DynamicClass;
                    
                    if (value?.Body.FirstOrDefault(x => x is AssignmentNode { IsPrivate: true, VariableName: "_value" }) is AssignmentNode var)
                        args[index] = var.Value;
                }
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