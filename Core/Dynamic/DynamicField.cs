using Core.AST;

namespace Core.Dynamic;

public class DynamicField(Variable var, FunctionNode? getFunction = null, FunctionNode? setFunction = null)
{
    public Variable PrivateVariable { get; } = var;
    public FunctionNode? GetFunction { get; } = getFunction;
    public FunctionNode? SetFunction { get; } = setFunction;

    public override string ToString()
    {
        return "Field";
    }
}