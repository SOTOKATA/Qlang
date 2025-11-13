using Qlang.AST;

namespace Qlang.Dynamic;

public class DynamicClass(string name)
{
    public readonly string Name = name;
    
    public readonly Dictionary<string, object> Variables = [];

    public List<ASTNode> Body = [];
}