using Core.Dynamic;

namespace Core.AST;

public class ASTContext
{
    public DynamicClass? Class { get; set; }
    
    public DynamicNamespace?  Namespace { get; set; }
    
    public DynamicFunction? Function { get; set; }

    public List<ASTBlock> Blocks { get; set; } = [];
    
    public ASTNode? CurrentNode { get; set; }
}