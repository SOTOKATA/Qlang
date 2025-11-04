using System.Globalization;

namespace Qlang.Dependencies.QlangDependencies.Functions;

public class Div : Function
{
    public override string GetName() => "div";

    public override string Execute(object[] args)
    {
        if (args.Length < 2)
            throw new ArgumentException("You must provide at least 2 arguments");
        
        if (!double.TryParse(args[0] as string, out double a) || !double.TryParse(args[1] as string, out double b))
            throw new Exception("The number of arguments must be double");
        
        if (b == 0)
            throw new Exception("The number of arguments must be 0");
        
        return (a / b).ToString(CultureInfo.InvariantCulture);
    }
}