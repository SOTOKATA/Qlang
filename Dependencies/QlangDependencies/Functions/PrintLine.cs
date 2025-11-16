namespace Qlang.Dependencies.QlangDependencies.Functions;

public class PrintLine : Function
{
    public override string GetName() => "";

    public override string Execute(object[] args)
    {
        List<object> list = args.ToList();
        
        list.Add(Environment.NewLine);

        args = list.ToArray();
        
        new Print().Execute(args);
        
        return "";
    }
}