using System.Text.RegularExpressions;

namespace Qlang.Dependencies.QlangDependencies.Functions;

public class Print : Function
{
    public override string GetName() => "write";

    public override string Execute(object[] args)
    {
        Console.Write(Regex.Unescape(string.Join("", args)));
        return "";
    }
}