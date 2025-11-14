namespace Qlang.Dependencies.QlangDependencies.Functions;

public class IsNumber : Function
{
    public override string GetName() => "isnumber";

    public override string Execute(object[] args)
    {
        if (args.Length == 0)
            throw new ArgumentException("Arguments is empty");
        
        Logger.Logger.Warn("Internal.isNumber: " + args[0].ToString().TryParseNumber(out _));
        
        return args[0].ToString().TryParseNumber(out _).ToString();
    }
}