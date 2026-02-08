using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class ObjectPointerNode(int line) : ASTNode(line)
{
    [Key(1)]
    
    public int NameId;
    
    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            ObjectPointerNode:
                                DebugIndex: {DebugIndex}
                                Name: {NameId}
                            """,
            indent);
    }

    public override ASTNode Clone()
    {
        return new ObjectPointerNode(DebugIndex)
        {
            NameId = NameId
        };
    }
}