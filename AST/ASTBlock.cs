using Qlang.Dynamic;

namespace Qlang.AST;

public abstract class ASTBlock : ASTNode
{
    public Dictionary<string, Variable> Variables { get; set; } = [];
}