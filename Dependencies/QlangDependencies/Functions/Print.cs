using System.Text.RegularExpressions;

namespace Qlang.Dependencies.QlangDependencies;

public class Print : Function
{
    public override string GetName() => "print";

    public override string Execute(object[] args)
    {
        Console.Write(Regex.Unescape(string.Join("", args)));

        return "";
    }
}