namespace Core.NativeLib.SystemLib.Classes;

public class ConsoleClass : IQlangClass
{
    public string Name { get; init; } = "console";

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