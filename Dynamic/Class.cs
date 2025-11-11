using Qlang.AST;

namespace Qlang.Dynamic;

public class DynamicClass(string name)
{
    public string Name = name;
    
    public Dictionary<string, object> Variables = [];

    public List<ASTNode> Body = [];
}