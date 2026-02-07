using System.Globalization;

namespace Core;

public static class UniversalParser
{
    public static bool TryParseNumber(this string? number, out double result)
    {
        return double.TryParse(number, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
    }
    
    public static double ParseNumber(this string? number)
    {
        return number != null ? 
            double.Parse(number, NumberStyles.Float, CultureInfo.InvariantCulture) 
            : 
            throw new Exception("Parse exception: string is null");
    }

    public static bool IsNumber(this string? number)
    {
        return number.TryParseNumber(out var _);
    }
}