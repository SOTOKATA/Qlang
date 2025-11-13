namespace Qlang.Dependencies.QlangDependencies.Functions;

public class SetForeColor : Function
{
    public override string GetName() => "fcolor";

    public override string Execute(object[] args)
    {
        if (args.Length == 0)
            throw new ArgumentException("The number of arguments must be greater than 0");
        
        if (Enum.TryParse(args[0].ToString(), true, out ConsoleColor color))
            Console.ForegroundColor = color;
        else throw new FormatException("The color argument must be a valid color");
        
        return "";
    }
}