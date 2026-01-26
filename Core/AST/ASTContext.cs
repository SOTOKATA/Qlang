using Core.Dynamic;

namespace Core.AST;

public class ASTContext
{
    public DynamicClass? Class { get; init; }
    
    public DynamicNamespace?  Namespace { get; init; }
    
    public DynamicFunction? Function { get; init; }

    public List<ASTBlock> Blocks { get; } = [];
    
    public ASTNode? CurrentNode { get; set; }
}