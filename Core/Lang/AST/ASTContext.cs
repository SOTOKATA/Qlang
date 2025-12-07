using Qlang.Core.Lang.Dynamic;

namespace Qlang.Core.Lang.AST;

public class ASTContext
{
    public DynamicClass? Class { get; set; }
    public DynamicFunction? Function { get; set; }

    public List<ASTBlock> Blocks { get; set; } = [];
    
    public ASTNode? CurrentNode { get; set; }
}