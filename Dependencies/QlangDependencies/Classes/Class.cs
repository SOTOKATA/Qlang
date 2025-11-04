namespace Qlang.Dependencies.QlangDependencies;

public abstract class Class
{
    public abstract string GetName();
    
    public abstract List<Function> GetFunctions();

    public virtual string GetDescription() => "";
}