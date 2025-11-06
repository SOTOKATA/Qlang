using System.Text.RegularExpressions;

namespace Qlang.Dependencies.QlangDependencies.Functions;

public class Print : Function
{
    public override string GetName() => "print";

    public override string Execute(object[] args)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(Regex.Unescape(string.Join("", args)));
        Console.ResetColor();
        
        return "";
    }
}