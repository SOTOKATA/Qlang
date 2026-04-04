namespace Core.NativeLib.SystemLib.Classes;

public class ConsoleClass : IQlangClass
{
    public string Name { get; init; } = "Console";

    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("Write", (Action<string?>)Console.Write),
            ("SetCursorVisible", (Action<bool>)(isVisible => Console.CursorVisible = isVisible)),
            ("GetCursorVisible", (Func<bool>)(() => Console.CursorVisible)),
            ("ReadKey", (Func<bool, string>)(intercept => {
                var keyInfo = Console.ReadKey(intercept);
                return char.IsControl(keyInfo.KeyChar)
                    ? keyInfo.Key.ToString().ToUpper()
                    : keyInfo.KeyChar.ToString();
            })),
            ("IsKeyAvailable", (Func<bool>)(() => Console.KeyAvailable)),
            ("Clear", (Action)Console.Clear),
            ("SetForeground", (Action<string>)SetForegroundColor),
            ("SetBackground", (Action<string>)SetBackgroundColor),
            ("GetForeground", (Func<string>)GetForegroundColor),
            ("GetBackground", (Func<string>)GetBackgroundColor),
            ("GetColors", (Func<List<object?>>)(() =>
                    {
                        return Enum.GetValues<ConsoleColor>().Select(c =>
                            {
                                var name = c.ToString();
                                return char.ToLowerInvariant(name[0]) + name[1..];
                            })
                            .Cast<object?>().ToList();

                    }
            )),
            ("ResetColors", Console.ResetColor),
            ("SetCursorPosition", (Action<int, int>)Console.SetCursorPosition),
            ("GetCurrentX", (Func<int>)(() => Console.GetCursorPosition().Left)),
            ("GetCurrentY", (Func<int>)(() => Console.GetCursorPosition().Top)),
            ("ReadLine", Console.ReadLine),
            ("GetWidth", (Func<double>)(() => Console.WindowWidth)),
            ("GetHeight", (Func<double>)(() => Console.WindowHeight)),
            ("SetWidth", (Action<int>)((d) => Console.WindowWidth = d)),
            ("SetHeight", (Action<int>)((d) => Console.WindowHeight = d)),
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