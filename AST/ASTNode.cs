namespace Qlang.AST;

public abstract class ASTNode
{
    public abstract string GetTree(string indent = "");
}