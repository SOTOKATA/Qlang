using MessagePack;

namespace Core.AST;

[MessagePackObject]
[Union(0, typeof(LineNode))]
[Union(1, typeof(ObjectPointerNode))]
[Union(2, typeof(FunctionPointerNode))]
[Union(3, typeof(NamespacePointerNode))]
[Union(4, typeof(CallNode))]
[Union(5, typeof(StringRefNode))]
[Union(6, typeof(NumberNode))]
[Union(7, typeof(BinaryOperationNode))]
[Union(8, typeof(AssignmentNode))]
[Union(9, typeof(FunctionNode))]
[Union(10, typeof(ClassNode))]
[Union(11, typeof(NamespaceNode))]
[Union(12, typeof(NumberRefNode))]
[Union(13, typeof(ReturnNode))]
[Union(14, typeof(IfNode))]
[Union(15, typeof(ForNode))]
[Union(16, typeof(SwitchNode))]
[Union(17, typeof(SwitchCaseNode))]
[Union(18, typeof(BooleanNode))]
[Union(19, typeof(CastNode))]
[Union(20, typeof(CollectionNode))]
[Union(21, typeof(KeywordNode))]
[Union(22, typeof(ParensNode))]
[Union(23, typeof(ProgramNode))]
[Union(24, typeof(WhileNode))]
[Union(25, typeof(ASTBlock))]
[Union(26, typeof(UsingNode))]
// ReSharper disable once InconsistentNaming
public abstract class ASTNode
{
    public abstract string GetTree(string indent = "");
    
    public abstract ASTNode Clone();
    
    protected static string DebugIndent(string text, string indent) => "\n" + string.Join("\n", text.Split('\n').Select(line => indent + line));
}