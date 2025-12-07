using Qlang.Core.Lang.AST;
using Qlang.Core.Lang.Dynamic.Exceptions;

namespace Qlang.Core.Lang.Compiler;

public static class Validator
{
    public static void CheckValidate(ProgramNode program)
    {
        CheckDuplicate(program);
    }
    
    private static void CheckDuplicate(ProgramNode program)
    {
        var classes = program.Statements
            .OfType<ClassNode>()
            .ToList();

        var @class = new ClassNode
        {
            Name = "Program",
            Body = program.Statements.OfType<FunctionNode>().Cast<ASTNode>().ToList()
        };
        
        classes.Add(@class);
        
        CheckDuplicateClasses(classes);

        foreach (var classNode in classes)
        {
            CheckDuplicateFunctions(classNode);
            CheckDuplicateAssignments(classNode);
        }
    }

    private static void CheckDuplicateClasses(List<ClassNode> classes)
    {
        var duplicateGroups = classes
            .GroupBy(f => new {
                f.Name,
            })
            .Where(g => g.Count() > 1);

        foreach (var group in duplicateGroups)
            throw new QlangCompileException($"Duplicate class found: '{group.Key.Name}'", group.First().Line, "Validator",  group.First().SourceFile);
    }

    private static void CheckDuplicateFunctions(ClassNode @class)
    {
        var functions = @class.Body
            .OfType<FunctionNode>()
            .ToList();

        var duplicateGroups = functions
            .GroupBy(f => new {
                f.Name,
                ParamCount = f.Parameters.Count,
            })
            .Where(g => g.Count() > 1);

        foreach (var group in duplicateGroups)
            throw new QlangCompileException(
                $"Duplicate function found: '{group.Key.Name}' in class '{@class.Name}'",
                group.First().Line,
                "Validator",
                group.First().SourceFile
            );
    }
    
    private static void CheckDuplicateAssignments(ClassNode @class)
    {
        var assignments = @class.Body
            .OfType<AssignmentNode>()
            .ToList();

        var duplicateGroups = assignments
            .GroupBy(f => new {
                f.VariableName
            })
            .Where(g => g.Count() > 1);

        foreach (var group in duplicateGroups)
            throw new QlangCompileException($"Duplicate assignment found: '{group.Key.VariableName}' in class '{@class.Name}'", group.First().Line, "Validator", group.First().SourceFile);
    }
}