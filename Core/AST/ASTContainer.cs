using Core.Tables;

namespace Core.AST;

public class ASTContainer : ASTNode
{
    public object? Value;

    public override string GetTree(string indent = "")
        => DebugIndent($"""
                       ASTContainer:
                            Value: {Value}
                       """, indent);

    public override ASTNode Clone()
    {
        return new ASTContainer
        {
            Value = Value,
        };
    }

    public override string ToTokenString(StringPoolTable stringPoolTable)
    {
        return $"{Value}";
    }
}