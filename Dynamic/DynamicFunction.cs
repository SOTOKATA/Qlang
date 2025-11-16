using Qlang.AST;

namespace Qlang.Dynamic;

public class DynamicFunction(string name)
{
    public readonly string Name = name;
    
    public List<string> Parameters = [];
    
    public readonly Dictionary<string, object> Variables = [];

    public List<ASTNode> Body = [];
}