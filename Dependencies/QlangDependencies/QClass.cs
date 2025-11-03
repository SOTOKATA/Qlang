namespace Qlang.Dependencies.QlangDependencies;

public abstract class QClass
{
    public abstract string GetName();
    
    public abstract List<QFunction> GetFunctions();

    public virtual string GetDescription() => "";
}