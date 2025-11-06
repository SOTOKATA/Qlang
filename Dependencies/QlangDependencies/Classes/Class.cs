using Qlang.Dependencies.QlangDependencies.Functions;

namespace Qlang.Dependencies.QlangDependencies.Classes;

public abstract class Class
{
    public abstract string GetName();
    
    public abstract List<Function> GetFunctions();

    public virtual string GetDescription() => "";
}