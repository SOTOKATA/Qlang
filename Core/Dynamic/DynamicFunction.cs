using Core.AST;

namespace Core.Dynamic;

public class DynamicFunction(string name)
{
    public readonly string Name = name;
    
    public readonly List<string> Parameters = [];
    
    public bool IsStatic = false;
    public bool IsPrivate = false;
    
    public readonly Dictionary<string, Variable> Variables = [];

    public readonly List<ASTNode> Body = [];
    
    public CallNode? ReturnType = null;

    public ASTContext? Context;

    public DynamicFunction Clone()
    {
        var dynamicFunction = new DynamicFunction(Name);
        
        dynamicFunction.Parameters.AddRange(Parameters.Select(x => x.ToString()).ToList());

        dynamicFunction.Body.AddRange(Body.Select(x => x.Clone()).ToList());

        dynamicFunction.ReturnType = (CallNode?)ReturnType?.Clone();
        
        dynamicFunction.IsStatic = IsStatic;
        dynamicFunction.IsPrivate = IsPrivate;
        
        foreach (var variable in Variables.Keys)
            dynamicFunction.Variables.Add(variable, Variables[variable].Clone());

        var fn = Context?.Function?.Clone();
        var pFn = Context?.ParentFunction?.Clone();

        if (Context is not null)
            dynamicFunction.Context = new ASTContext()
            {
                ParentFunction = pFn,
                Function = fn,
                Class = Context.Class,
                Namespace = Context.Namespace,
                Blocks = Context.Blocks.Select(x => (ASTBlock)x.Clone()).ToList(),
                AllowPrivateCall = Context.AllowPrivateCall,
                IsReturn = Context.IsReturn,
                IsBreak = Context.IsBreak,
                IsContinue = Context.IsContinue,
            };
        
        return dynamicFunction;
    }
}