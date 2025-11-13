using Qlang.Dependencies.QlangDependencies.Classes;
using Math = Qlang.Dependencies.QlangDependencies.Classes.Math;

namespace Qlang.Dependencies.QlangDependencies;

public class Namespace
{
    public static List<Class> GetClassList() =>
    [
        new Term(), new Math(), new Number(), new Throw()
    ];
}