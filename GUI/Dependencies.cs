using Raylib_cs;

namespace GUI;

public class Dependencies
{
    public static Color GetColor(float h, float s, float v)
        => Color.FromHSV(h, s, v);
}