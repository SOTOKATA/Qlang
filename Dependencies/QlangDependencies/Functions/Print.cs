namespace Qlang.Dependencies.QlangDependencies;

public class Print : Function
{
    public override string GetName() => "print";

    public override string Execute(object[] args)
    {
        Console.WriteLine(string.Join(" ", args));

        return "";
    }
}