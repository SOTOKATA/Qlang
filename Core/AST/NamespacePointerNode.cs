using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class NamespacePointerNode(int line) : ASTNode(line)
{
    [Key(1)]
    
    public int NameId { get; set; }
    
    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            NamespacePointerNode:
                                DebugIndex: {DebugIndex}
                                Name: {NameId}.
                            """, indent);
    }

    public override ASTNode Clone()
    {
        return new NamespacePointerNode(DebugIndex)
        {
            NameId = NameId,
        };
    }
}