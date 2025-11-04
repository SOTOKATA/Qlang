namespace Qlang.Dependencies.QlangDependencies.Classes;

public class Term : Class
{
    public override string GetName() => "Term";

    public override List<Function> GetFunctions() => [
        new Print(), new Read()
    ];
}