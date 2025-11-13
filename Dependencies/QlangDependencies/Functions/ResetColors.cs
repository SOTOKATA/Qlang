namespace Qlang.Dependencies.QlangDependencies.Functions;

public class ResetColors : Function
{
    public override string GetName() => "resetcolors";

    public override string Execute(object[] args)
    {
        Console.BackgroundColor = ConsoleColor.Red;
        return "";
    }
}