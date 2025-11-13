namespace Qlang.Dependencies.QlangDependencies.Functions;

public class ThrowException : Function
{
    public override string GetName() => "exception";

    public override string Execute(object[] args)
    {
        throw new System.Exception(args.Length > 0 ? args[0].ToString() : "");
    }
}