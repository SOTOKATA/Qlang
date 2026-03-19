using Core.Tables;
using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class FunctionPointerNode : CallPartNode
{
    [Key(2)]
    
    public required List<ASTNode> Arguments;
    
    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            FunctionPointerNode:
                                Name: {NameId}
                                Arguments: [{string.Join(",\n", Arguments.Select(x => x.GetTree("\t")))}]
                                IsNullable: {IsNullable}
                            """, indent);
    }
    
    public override string ToTokenString(StringPoolTable stringPoolTable)
        => $"{stringPoolTable[NameId]}({string.Join(", ", Arguments.Select(x => x.ToTokenString(stringPoolTable)))})" + (IsNullable ? "?" : string.Empty);

    public override ASTNode Clone()
    {
        return new FunctionPointerNode
        {
            NameId = NameId,
            Arguments = [..Arguments.ConvertAll(arg => arg.Clone())],
            IsNullable = IsNullable,
        };
    }
}