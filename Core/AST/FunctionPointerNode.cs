namespace Core.AST;

public class FunctionPointerNode : ASTNode
{
    public string Name;
    public List<ASTNode> Arguments;
    
    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            FunctionPointerNode:
                                Name: {Name}
                                Arguments: [{string.Join(",\n", Arguments.Select(x => x.GetTree("\t\t")))}]
                            """, indent);
    }

    public override ASTNode Clone()
    {
        return new FunctionPointerNode
        {
            Name = Name,
            Arguments = [..Arguments.ConvertAll(arg => arg.Clone())],
            SourceFile =  SourceFile, 
            Line =  Line 
        };
    }
}