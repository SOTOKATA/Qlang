using System.Globalization;

namespace Qlang.Dependencies.QlangDependencies.Functions;

public class Mult : Function
{
    public override string GetName() => "mult";

    public override string Execute(object[] args)
    {
        if (args.Length < 2)
            throw new ArgumentException("You must provide at least 2 arguments");
        
        if (!(args[0].ToString()).TryParseNumber(out double a) || 
            !(args[1].ToString()).TryParseNumber(out double b))
            throw new Exception($"The arguments must be number ('{args[0]}', '{args[1]}')");
        
        return (a * b).ToString(CultureInfo.InvariantCulture);
    }
}