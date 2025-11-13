using Qlang.Dependencies.QlangDependencies.Functions;

namespace Qlang.Dependencies.QlangDependencies.Classes;

public class Throw : Class
{
    public override string GetName() => "Throw";

    public override List<Function> GetFunctions() =>
    [
        new ThrowException()
    ];
}