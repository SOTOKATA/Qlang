using System.Globalization;
using Qlang.Compiler;

namespace Qlang.Dependencies;

public static class UniversalParser
{
    public static bool TryParseNumber(this string? number, out double result)
    {
        return double.TryParse(number, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
    }
    
    public static double ParseNumber(this string? number)
    {
        if (number != null)
            return double.Parse(number, NumberStyles.Float, CultureInfo.InvariantCulture);
        
        throw new Exception("Parse exception: string is null");
    }

    public static bool IsNumber(this string? number)
    {
        return number.TryParseNumber(out var _);
    }
}