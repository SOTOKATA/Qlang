namespace Qlang.Dependencies.QlangDependencies.Functions;

public abstract class Function
{
    public abstract string GetName();
    
    public abstract string Execute(object[] args);
    
    public virtual string GetDescription() => "This is function description.";
}