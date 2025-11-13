using System.Globalization;

namespace Qlang.Dependencies.QlangDependencies.Functions;

public class GetMaxValue : Function
{
    public override string GetName() => "getmaxvalue";

    public override string Execute(object[] args)
    {
        return double.MaxValue.ToString(CultureInfo.InvariantCulture);
    }
}