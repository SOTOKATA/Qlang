namespace Qlang.Dependencies.QlangDependencies.Functions;

public class SetBackColor : Function
{
    public override string GetName() => "bcolor";

    public override string Execute(object[] args)
    {
        if (args.Length == 0)
            throw new ArgumentException("The number of arguments must be greater than 0");
        
        if (Enum.TryParse(args[0].ToString(), true, out ConsoleColor color))
            Console.BackgroundColor = color;
        else throw new FormatException("The color argument must be a valid color");

        return "";
    }
}