using System.Globalization;
using Qlang.Compiler;

namespace Qlang.Dependencies;

public static class UniversalParser
{
    public static bool TryParseNumber(this string number, out double result)
    {
        return double.TryParse(number, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
    }

    public static bool IsNumber(this string number)
    {
        return double.TryParse(number, NumberStyles.Any, CultureInfo.InvariantCulture, out double _);
    }
}