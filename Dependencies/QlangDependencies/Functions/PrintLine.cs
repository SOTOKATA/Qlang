namespace Qlang.Dependencies.QlangDependencies.Functions;

public class PrintLine : Function
{
    public override string GetName() => "println";

    public override string Execute(object[] args)
    {
        Console.WriteLine(string.Join("", args));

        return "";
    }
}