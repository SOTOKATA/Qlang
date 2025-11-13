using System.Globalization;

namespace Qlang.Dependencies.QlangDependencies.Functions;

public class GetMinValue : Function
{
    public override string GetName() => "getminvalue";

    public override string Execute(object[] args)
    {
        return double.MinValue.ToString(CultureInfo.InvariantCulture);
    }
}