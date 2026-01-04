using Core.Dynamic;

namespace Core.AST;

public abstract class ASTBlock : ASTNode
{
    public Dictionary<string, Variable> Variables { get; set; } = [];
}