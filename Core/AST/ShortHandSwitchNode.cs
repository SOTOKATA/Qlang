using Core.Tables;
using MessagePack;

namespace Core.AST;

[MessagePackObject]
public class ShortHandSwitchCase : ASTNode
{
    [Key(0)]
    public int? BinaryOperationId { get; set; }
    
    [Key(1)]
    public required ASTNode Key { get; set; }

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            ShortHandSwitchNode:
                                BinaryOperationId: {BinaryOperationId}
                                Key: {Key.GetTree("\t")}
                            """, indent);
    }

    public override ASTNode Clone() => new ShortHandSwitchCase
    {
        BinaryOperationId = BinaryOperationId,
        Key = Key.Clone()
    };

    public override string ToTokenString(StringPoolTable stringPoolTable)
    {
        return $"{(BinaryOperationId is not null ? stringPoolTable[BinaryOperationId ?? 0] : "")} {Key.ToTokenString(stringPoolTable)}";
    }
}

[MessagePackObject]
public class ShortHandSwitchNode : ASTNode
{
    [Key(0)]
    public required ASTNode Value { get; set; }

    [Key(1)] 
    public required Dictionary<ShortHandSwitchCase, ASTNode> Cases { get; set; } = [];
    
    [Key(2)]
    public required ASTNode? Default { get; set; }
    
    
    public override string GetTree(string indent = "") => DebugIndent($"""
                            ShortHandSwitchNode:
                                Value: {Value.GetTree("\t")}
                                Default: {Default?.GetTree("\t")}
                                Cases: {Cases.Select(x => x.Value.GetTree("\t"))}
                            """, indent);

    public override ASTNode Clone() => new ShortHandSwitchNode
    {
        Value = Value.Clone(),
        Cases = Cases.Select(x => new KeyValuePair<ShortHandSwitchCase, ASTNode>((ShortHandSwitchCase)x.Key.Clone(), x.Value.Clone())).ToDictionary(),
        Default = Default?.Clone()
    };

    public override string ToTokenString(StringPoolTable stringPoolTable)
    {
        return
            $"switch {Value.ToTokenString(stringPoolTable)}: {{";
    }
}