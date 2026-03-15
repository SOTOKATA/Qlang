using Core.Tables;
using MessagePack;


namespace Core.AST;

[MessagePackObject]
public class BinaryOperationNode : ASTNode
{
    [Key(0)]
    
    public ASTNode? Left { get; set; }

    [Key(1)]
    
    public int OperatorId { get; set; }
    [Key(2)]
    
    public ASTNode? Right { get; set; }

    public override ASTNode Clone()
    {
        return new BinaryOperationNode
        {
            Left = Left?.Clone(),
            OperatorId = OperatorId,
            Right = Right?.Clone()
        };
    }

    public override string ToTokenString(StringPoolTable stringPoolTable)
    => Left?.ToTokenString(stringPoolTable) + " " + stringPoolTable[OperatorId] + " " +  Right?.ToTokenString(stringPoolTable);

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            BinaryOperationNode:
                                Operator: {OperatorId}
                                Left: {Left?.GetTree("\t") ??  "<undefined>"}
                                Left: {Right?.GetTree("\t") ??  "<undefined>"}
                            """, indent);
    }
}