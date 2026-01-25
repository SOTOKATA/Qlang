using Core;
using Core.AST;
using Core.Exceptions;

namespace Compiler;

public class Validator(SourceFileTable sourceFileTable)
{
    private SourceFileTable _sourceFileTable = sourceFileTable;
    
    public void CheckValidate(ProgramNode program)
    {
        CheckDuplicate(program);
    }
    
    
    private void CheckDuplicate(ProgramNode program)
    {
        var classes = program.Statements
            .OfType<ClassNode>()
            .ToList();

        foreach (var @namespace in program.Statements.OfType<NamespaceNode>())
            classes.AddRange(Parser.GetClassesFromNamespaceRecursively(@namespace));

        CheckDuplicateClasses(classes);

        foreach (var classNode in classes)
        {
            CheckDuplicateFunctions(classNode);
            CheckDuplicateAssignments(classNode);
        }
    }

    private void CheckDuplicateClasses(List<ClassNode> classes)
    {
        var duplicateGroups = classes
            .GroupBy(f => new {
                f.Name,
            })
            .Where(g => g.Count() > 1);

        foreach (var group in duplicateGroups)
            throw new QlangCompileException($"Duplicate class found: '{group.Key.Name}'", group.First().Line, "Validator",  _sourceFileTable[group.First().SourceFileId]);
    }

    private void CheckDuplicateFunctions(ClassNode @class)
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
                _sourceFileTable[group.First().SourceFileId]
            );
    }
    
    private void CheckDuplicateAssignments(ClassNode @class)
    {
        var assignments = @class.Body
            .OfType<AssignmentNode>()
            .ToList();

        var duplicateGroups = assignments
            .GroupBy(f => new {
                VariableName = f.GetLastName()
            })
            .Where(g => g.Count() > 1);

        foreach (var group in duplicateGroups)
            throw new QlangCompileException($"Duplicate assignment found: '{group.Key.VariableName}' in class '{@class.Name}'", group.First().Line, "Validator", _sourceFileTable[group.First().SourceFileId]);
    }
}