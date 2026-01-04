using Core.NativeLib;
using Raylib_cs;

namespace GUI;

public class GuiLib : IQlangLib
{
    public string Name { get; } = "UISupport";
    public string Version { get; } = "0.0.1";
    public string Author { get; } = "SOTOKATA";
    public string Class { get; } = "window";
    public string Namespace { get; } = "gui";
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("init", (Action<int, int, string>)Raylib.InitWindow),
            ("close", Raylib.CloseWindow),
            ("should_close", Raylib.WindowShouldClose),
            ("begin_drawing", Raylib.BeginDrawing),
            ("end_drawing", Raylib.EndDrawing),
            ("clear_background", Raylib.ClearBackground),
            ("draw_text", (Action<string, int, int, int, Color>)(Raylib.DrawText)),
            ("get_hsv_color", Color.FromHSV),
        ];
    }
}