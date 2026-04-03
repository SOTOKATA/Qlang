using Core.Dynamic;
using Core.Tables;
using MessagePack;

namespace Core.AST;

[MessagePackObject]
public class FieldNode : ASTNode
{

    [Key(0)]
    public required AssignmentNode PrivateVariable { get; set; }
    
    [Key(1)]
    public FunctionNode? GetFunction { get; set; }
    [Key(2)]
    public FunctionNode? SetFunction { get; set; }
    
    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                           FieldNode:
                                PrivateVariable: {PrivateVariable.GetTree("\t")}
                                GetFunction: {GetFunction?.GetTree("\t")}
                                GetFunction: {SetFunction?.GetTree("\t")}
                           """, indent);
    }

    public override ASTNode Clone()
    {
        return new FieldNode
        {
            PrivateVariable = (AssignmentNode)PrivateVariable.Clone(),
            GetFunction = (FunctionNode?)GetFunction?.Clone(),
            SetFunction = (FunctionNode?)SetFunction?.Clone(),
        };
    }

    public override string ToTokenString(StringPoolTable stringPoolTable)
    {
        return $$"""
                field({{stringPoolTable[PrivateVariable.GetLastNameId()]}}) { {{GetFunction?.ToTokenString(stringPoolTable)}} {{SetFunction?.ToTokenString(stringPoolTable)}} }
                """;
    }
}