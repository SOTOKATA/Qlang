namespace Core.NativeLib.SystemLib;

public class ConsoleLib : IQlangLib
{
    public string Name { get; } = "CSharpConsole";

    public string Version { get; } = "0.0.0";
    
    public string Author { get; } = "SOTOKATA";

    public string Class { get; } = "console";
    
    public string Namespace { get; } = "lib";


    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("write", (Action<string?>)Console.Write),
            ("cursor_visible", (Action<bool>)(isVisible => Console.CursorVisible = isVisible)),
            ("key", (Func<bool, string>)(intercept => Console.ReadKey(intercept).KeyChar.ToString())),
            ("key_available", (Func<bool>)(() => Console.KeyAvailable)),
            ("clear", (Action)Console.Clear),
            ("foreground", (Action<string>)SetForegroundColor),
            ("background", (Action<string>)SetBackgroundColor),
            ("reset_color", Console.ResetColor),
            ("cursor_position", (Action<int, int>)Console.SetCursorPosition),
            ("read", Console.ReadLine),
            ("width", (Func<double>)(() => Console.WindowWidth)),
            ("height", (Func<double>)(() => Console.WindowHeight))
        ];
    }

    private static void SetForegroundColor(string line)
    {
        if (Enum.TryParse(line, true, out ConsoleColor color))
            Console.ForegroundColor = color;
    }

    private static void SetBackgroundColor(string line)
    {
        if (Enum.TryParse(line, true, out ConsoleColor color))
            Console.ForegroundColor = color;
    }
}