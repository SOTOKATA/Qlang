namespace Core.AST;

public class UsingNode(int line, int sfId) : ASTNode(line, sfId)
{
    public required CallNode CallPath { get; set; }
    
    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            UsingNode:
                                CallPath: {CallPath.GetTree("\t\t")}
                            """, indent);
    }

    public override ASTNode Clone()
    {
        return new UsingNode(line, SourceFileId)
        {
            CallPath = CallPath
        };
    }
}