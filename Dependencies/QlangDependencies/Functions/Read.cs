namespace Qlang.Dependencies.QlangDependencies;

public class Read : Function
{
    public override string GetName() => "read";

    public override string Execute(object[] args)
    {
        return Console.ReadLine();
    }
}