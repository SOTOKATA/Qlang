namespace Core.NativeLib.SystemLib.Classes;

public class ConsoleClass : IQlangClass
{
    public string Name { get; init; } = "console";

    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("write", (Action<string?>)Console.Write),
            ("setCursorVisible", (Action<bool>)(isVisible => Console.CursorVisible = isVisible)),
            ("getCursorVisible", (Func<bool>)(() => Console.CursorVisible)),
            ("key", (Func<bool, string>)(intercept => {
                var keyInfo = Console.ReadKey(intercept);
                return char.IsControl(keyInfo.KeyChar)
                    ? keyInfo.Key.ToString().ToUpper()
                    : keyInfo.KeyChar.ToString();
            })),
            ("keyAvailable", (Func<bool>)(() => Console.KeyAvailable)),
            ("clear", (Action)Console.Clear),
            ("setForeground", (Action<string>)SetForegroundColor),
            ("setBackground", (Action<string>)SetBackgroundColor),
            ("getForeground", (Func<string>)GetForegroundColor),
            ("getBackground", (Func<string>)GetBackgroundColor),
            ("colors", (Func<List<object?>>)(() =>
                    {
                        return Enum.GetValues<ConsoleColor>().Select(c =>
                            {
                                var name = c.ToString();
                                return char.ToLowerInvariant(name[0]) + name[1..];
                            })
                            .Cast<object?>().ToList();

                    }
            )),
            ("resetColor", Console.ResetColor),
            ("cursorPosition", (Action<int, int>)Console.SetCursorPosition),
            ("getCurrentX", (Func<int>)(() => Console.GetCursorPosition().Left)),
            ("getCurrentY", (Func<int>)(() => Console.GetCursorPosition().Top)),
            ("read", Console.ReadLine),
            ("getWidth", (Func<double>)(() => Console.WindowWidth)),
            ("getHeight", (Func<double>)(() => Console.WindowHeight)),
            ("setWidth", (Action<int>)((d) => Console.WindowWidth = d)),
            ("setHeight", (Action<int>)((d) => Console.WindowHeight = d)),
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
    
    private static string GetForegroundColor()
    {
        var rawName = Console.ForegroundColor.ToString();
        return char.ToLower(rawName[0]) + rawName[1..];
    }

    private static string GetBackgroundColor()
    {
        var rawName = Console.BackgroundColor.ToString();
        return char.ToLower(rawName[0]) + rawName[1..];
    }
}