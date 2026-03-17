using Core.Tables;
using MessagePack;

namespace Core.AST;

[MessagePackObject]
public class ShortHandIfNode : ASTNode
{
    [Key(0)]
    public required ASTNode Condition { get; set; }
    
    [Key(1)]
    
    public required ASTNode Then { get; set; }
    
    [Key(2)]
    
    public required ASTNode Else { get; set; }
    
    public override string GetTree(string indent = "") => DebugIndent($"""
                            ShortHandIfNode:
                                Condition: {Condition.GetTree("\t")}
                                Then: {Then.GetTree("\t")}
                                Else: {Else.GetTree("\t")}
                            """, indent);

    public override ASTNode Clone() => new ShortHandIfNode
    {
        Condition = Condition.Clone(),
        Then = Then.Clone(),
        Else = Else.Clone(),
    };

    public override string ToTokenString(StringPoolTable stringPoolTable)
    {
        return
            $"if {Condition.ToTokenString(stringPoolTable)} ? {Then.ToTokenString(stringPoolTable)} : {Else.ToTokenString(stringPoolTable)}";
    }
}