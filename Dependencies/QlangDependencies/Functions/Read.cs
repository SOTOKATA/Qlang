namespace Qlang.Dependencies.QlangDependencies.Functions;

public class Read : Function
{
    public override string GetName() => "read";

    public override string Execute(object[] args)
    {
        // Console.ForegroundColor = ConsoleColor.DarkYellow;
        var input = Console.ReadLine();
        // Console.ResetColor();
        return input;
    }
}