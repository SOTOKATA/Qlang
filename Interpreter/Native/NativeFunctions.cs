using Qlang.Dependencies;
using Qlang.Interpreter.Native;

namespace Qlang.Interpreter;

public class NativeFunctionRegistry
{
    private readonly Dictionary<string, Delegate> _functions = new();
    
    public NativeFunctionRegistry()
    {
        Register("to_string_number", (Func<double, string, string>)((o, pattern) => o.ToString(pattern)));
      
        // Console
        Register("cmd_write", (Action<string>)Console.Write);
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
        Register("exception", (Action<string>)(msg => throw new Exception(msg)));
        
        // String
        Register("str_length", (Func<string, int>)(NativeString.GetLength));
        Register("str_null_or_empty", (Func<string, bool>)(string.IsNullOrEmpty));
        Register("str_null_or_white_space", (Func<string, bool>)(string.IsNullOrWhiteSpace));
        Register("str_trim", (Func<string, string>)(msg => msg.Trim()));
        Register("str_trim_start", (Func<string, string>)(msg => msg.TrimStart()));
        Register("str_trim_end", (Func<string, string>)(msg => msg.TrimEnd()));
        Register("str_sub_string", (Func<string, int, int, string>)((msg, start, length) => msg.Substring(start, length)));
        // Register("str_length", (Action<string>)(msg => msg.Length);
        
        // Time
        Register("time_wait", (Action<int>)(Thread.Sleep));
        
        // Array
        Register("list_create", (Func<List<object>>)(() => []));
        Register("list_insert", (Action<List<object>, int, object>)((list, pos, item) => list.Insert(pos, item)));
        Register("list_add", (Action<List<object>, object>)((list, item) => list.Add(item)));
        Register("list_get", (Func<List<object>, int, object>)((list, idx) => list[idx]));
        Register("list_is", (Func<object, bool>)(obj => obj is List<object>));
        Register("list_set", (Action<List<object>, int, object>)((list, idx, val) => list[idx] = val));
        Register("list_count", (Func<List<object>, int>)(list => list.Count));
        Register("list_clear", (Action<List<object>>)(list => list.Clear()));
        Register("list_contains", (Func<List<object>, object, bool>)((list, item) => list.Contains(item)));
        Register("list_remove_at", (Action<List<object>, int>)((list, idx) => list.RemoveAt(idx)));
        Register("list_index_of", (Func<List<object>, object, int>)((list, item) => list.IndexOf(item)));
        
        // Parser
        Register("parse_int", (Func<object, int>)(obj => (int)NativeParser.Parse(obj, "int")));
        Register("parse_float", (Func<object, double>)(obj => (double)NativeParser.Parse(obj, "float")));
        Register("parse_number", (Func<object, double>)(obj => (double)NativeParser.Parse(obj, "double")));
        Register("parse_string", (Func<object, string>)(obj => (string)NativeParser.Parse(obj, "string")));
    }

    private void Register(string name, Delegate handler)
    {
        _functions[name] = handler;
    }
    
    public object? Call(string name, object?[]? args)
    {
        if (!_functions.TryGetValue(name, out var func))
            throw new Exception($"Function '{name}' not found");

        Logger.Logger.Log($"NativeCall: $name='{name}'");
        if (args is not null)
        {
            var arguments = func.Method.GetParameters();

            string debug = "NativeCall: $args=\n";
            for (var index = 0; index < arguments.Length; index++)
            {
                var type = arguments[index];

                debug += $"${index}: Type: {type.ParameterType} ";

                if (type.ParameterType == typeof(int))
                    args[index] = int.Parse(args[index].ToString());
                else if (type.ParameterType == typeof(double))
                    args[index] = args[index].ToString().ParseNumber();
            }

            debug += "\n";

            for (var index = 0; index < args.Length; index++)
            {
                var arg = args[index];
                debug += $"${index}: '{arg}' $type: '{(arg is null ? "NULL" : arg.GetType().Name)}' ";
            }


            Logger.Logger.Log("Call.Arguments: " + debug);
        }
        else
            Logger.Logger.Log("NativeCall: $args=NULL");

        return func.DynamicInvoke(args);
    }
}