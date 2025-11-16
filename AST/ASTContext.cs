using Qlang.Dynamic;

namespace Qlang.AST;

public class ASTContext
{
    public DynamicClass? Class { get; set; }
    public DynamicFunction? Function { get; set; }
    public ASTNode CurrentNode { get; set; }
}