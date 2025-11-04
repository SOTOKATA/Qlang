using Qlang.Dependencies.QlangDependencies.Functions;

namespace Qlang.Dependencies.QlangDependencies.Classes;

public class Math : Class
{
    public override string GetName() => "Math";

    public override List<Function> GetFunctions() => [
        new Sum(), new Sub(), new Div(), new Mult()
    ];
}