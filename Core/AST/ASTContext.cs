using Core.Dynamic;

namespace Core.AST;

public class ASTContext
{
    public DynamicClass? Class { get; init; }
    
    public DynamicNamespace?  Namespace { get; init; }
    
    public DynamicFunction? Function { get; init; }
    
    public DynamicFunction? ParentFunction { get; set; }

    public List<ASTBlock> Blocks { get; init; } = [];
    
    public ASTNode? CurrentNode { get; set; }
    public int CurrentDebugIndex { get; set; } = -1;

    public bool IsReturn = false;
    public bool IsBreak = false;
    public bool IsContinue = false;
    public object? ReturnValue = null;
    public bool AllowPrivateCall = false;

    public ASTContext Copy()
    {
        return new ASTContext
        {
            Class = Class,
            Namespace = Namespace,
            Function = Function,
            ParentFunction = ParentFunction,
            Blocks = Blocks,

            CurrentNode = CurrentNode,
            CurrentDebugIndex = CurrentDebugIndex,

            IsReturn = IsReturn,
            IsBreak = IsBreak,
            IsContinue = IsContinue,
            ReturnValue = ReturnValue,
            AllowPrivateCall = AllowPrivateCall,
        };
    }
}