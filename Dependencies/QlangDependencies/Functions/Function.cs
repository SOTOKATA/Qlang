namespace Qlang.Dependencies.QlangDependencies;

public abstract class Function
{
    public abstract string GetName();
    
    public abstract string Execute(object[] args);
    
    public virtual string GetDescription() => "This is function description.";
}