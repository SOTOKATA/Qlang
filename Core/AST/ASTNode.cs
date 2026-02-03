using MessagePack;


namespace Core.AST;

[MessagePackObject]
[Union(0, typeof(AssignmentNode))]
[Union(1, typeof(BinaryOperationNode))]
[Union(2, typeof(BooleanNode))]
[Union(3, typeof(CallNode))]
[Union(4, typeof(CastNode))]
[Union(5, typeof(ClassNode))]
[Union(6, typeof(CollectionNode))]
[Union(7, typeof(ForNode))]
[Union(8, typeof(FunctionNode))]
[Union(9, typeof(FunctionPointerNode))]
[Union(10, typeof(IfNode))]
[Union(11, typeof(KeywordNode))]
[Union(12, typeof(NamespaceNode))]
[Union(13, typeof(NamespacePointerNode))]
[Union(14, typeof(NumberNode))]
[Union(15, typeof(NumberRefNode))]
[Union(16, typeof(ObjectPointerNode))]
[Union(17, typeof(ParensNode))]
[Union(18, typeof(ProgramNode))]
[Union(19, typeof(ReturnNode))]
[Union(20, typeof(StringRefNode))]
[Union(21, typeof(SwitchNode))]
[Union(22, typeof(SwitchCaseNode))]
[Union(23, typeof(UsingNode))]
[Union(24, typeof(WhileNode))]
[Union(25, typeof(ASTBlock))]
// ReSharper disable once InconsistentNaming
public abstract class ASTNode(int debugIndex = -1)
{
    [Key(0)]
     
    public int DebugIndex { get; set; } = debugIndex;
    
    public abstract string GetTree(string indent = "");
    
    public abstract ASTNode Clone();
    
    protected static string DebugIndent(string text, string indent) => "\n" + string.Join("\n", text.Split('\n').Select(line => indent + line));
}