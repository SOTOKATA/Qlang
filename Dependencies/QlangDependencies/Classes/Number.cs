using Qlang.Dependencies.QlangDependencies.Functions;

namespace Qlang.Dependencies.QlangDependencies.Classes;

public class Number : Class
{
    public override string GetName() => "Number";

    public override List<Function> GetFunctions() =>
    [
        new IsNumber(), new GetMinValue(), new GetMaxValue()
    ];
}