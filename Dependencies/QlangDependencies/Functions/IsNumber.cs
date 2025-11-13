namespace Qlang.Dependencies.QlangDependencies.Functions;

public class IsNumber : Function
{
    public override string GetName() => "isnumber";

    public override string Execute(object[] args)
    {
        if (args.Length == 0)
            throw new ArgumentException("Arguments is empty");
        
        return double.TryParse(args[0].ToString(), out double _).ToString();
    }
}