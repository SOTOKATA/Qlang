using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class FunctionPointerNode : ASTNode
{
    [Key(0)]
    
    public required int NameId;
    [Key(1)]
    
    public required List<ASTNode> Arguments;
    
    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            FunctionPointerNode:
                                Name: {NameId}
                                Arguments: [{string.Join(",\n", Arguments.Select(x => x.GetTree("\t\t")))}]
                            """, indent);
    }

    public override ASTNode Clone()
    {
        return new FunctionPointerNode
        {
            NameId = NameId,
            Arguments = [..Arguments.ConvertAll(arg => arg.Clone())]
        };
    }
}