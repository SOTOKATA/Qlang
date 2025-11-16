using Qlang.AST;

namespace Qlang.Dynamic;

public class DynamicFunction(string name)
{
    public readonly string Name = name;
    
    public List<string> Parameters = [];
    
    public bool IsStatic = false;
    public bool IsPrivate = false;
    
    public readonly Dictionary<string, Variable> Variables = [];

    public List<ASTNode> Body = [];
}