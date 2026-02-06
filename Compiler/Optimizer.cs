using Core.AST;

namespace Compiler;

public class Optimizer
{
    public ProgramNode Optimize(ProgramNode program)
    {
        program.DebugIndex = 0;
        
        program.Statements.RemoveAll(x => x is not NamespaceNode);
        
        for (var i = 0; i < program.Statements.Count; i++)
            program.Statements[i] = OptimizeNamespace((NamespaceNode)program.Statements[i]);
        
        return program;
    }

    private static FunctionNode OptimizeFunction(FunctionNode function)
    {
        function.Body.RemoveAll(x => x is FunctionNode or ClassNode or NamespaceNode);
        return function;
    }
    
    private static ClassNode OptimizeClass(ClassNode @class)
    {
        @class.Body.RemoveAll(x => x is not FunctionNode && x is not AssignmentNode);    
        
        for (var index = 0; index < @class.Body.Count; index++)
        {
            var item = @class.Body[index];
            @class.Body[index] = item switch
            {
                FunctionNode subFunction => OptimizeFunction(subFunction),
                _ => item
            };
        }
        return @class;
    }
    
    private static NamespaceNode OptimizeNamespace(NamespaceNode @namespace)
    {
        @namespace.Body.RemoveAll(x => x is not NamespaceNode && x is not AssignmentNode && x is not FunctionNode && x is not ClassNode);
        for (var index = 0; index < @namespace.Body.Count; index++)
        {
            var item = @namespace.Body[index];
            @namespace.Body[index] = item switch
            {
                NamespaceNode subNamespace => OptimizeNamespace(subNamespace),
                ClassNode subClass => OptimizeClass(subClass),
                FunctionNode subFunction => OptimizeFunction(subFunction),
                _ => item
            };
        }
        return @namespace;
    }

}