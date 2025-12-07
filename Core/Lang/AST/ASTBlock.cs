using Qlang.Core.Lang.Dynamic;

namespace Qlang.Core.Lang.AST;

public abstract class ASTBlock : ASTNode
{
    public Dictionary<string, Variable> Variables { get; set; } = [];
}