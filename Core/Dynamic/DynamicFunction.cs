using Core.AST;

namespace Core.Dynamic;

public class DynamicFunction(string name)
{
    public readonly string Name = name;
    
    public readonly List<string> Parameters = [];
    
    public bool IsStatic = false;
    public bool IsPrivate = false;
    
    public readonly Dictionary<string, Variable> Variables = [];

    public List<ASTNode> Body = [];
    
    public CallNode? ReturnType = null;

    public ASTContext? Context;
}