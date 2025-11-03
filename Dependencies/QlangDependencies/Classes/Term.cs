namespace Qlang.Dependencies.QlangDependencies.Classes;

public class Term : QClass
{
    public override string GetName() => "Term";

    public override List<QFunction> GetFunctions() => [
        new Print()
    ];
}