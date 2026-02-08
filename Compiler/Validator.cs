using Core.AST;
using Core.Exceptions;
using Core.Tables;

namespace Compiler;

public class Validator(SourceFileTable sourceFileTable, DebugTable debugTable, StringPoolTable stringPoolTable)
{
    
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
                Name = f.NameId,
            })
            .Where(g => g.Count() > 1);

        foreach (var group in duplicateGroups)
            throw new QlangCompileException($"Duplicate class found: '{group.Key.Name}'", GetDebug(group.First()), "Validator");
    }

    private void CheckDuplicateFunctions(ClassNode @class)
    {
        var functions = @class.Body
            .OfType<FunctionNode>()
            .ToList();

        var duplicateGroups = functions
            .GroupBy(f => new {
                Name = f.NameId,
                ParamCount = f.Parameters.Count,
            })
            .Where(g => g.Count() > 1);

        foreach (var group in duplicateGroups)
            throw new QlangCompileException(
                $"Duplicate function found: '{group.Key.Name}' in class '{@class.NameId}'",
                GetDebug(group.First()),
                "Validator"
            );
    }
    
    private void CheckDuplicateAssignments(ClassNode @class)
    {
        var assignments = @class.Body
            .OfType<AssignmentNode>()
            .ToList();

        var duplicateGroups = assignments
            .GroupBy(f => new {
                VariableName = stringPoolTable[f.GetLastNameId()]
            })
            .Where(g => g.Count() > 1);

        foreach (var group in duplicateGroups)
            throw new QlangCompileException($"Duplicate assignment found: '{group.Key.VariableName}' in class '{stringPoolTable[@class.NameId]}'", GetDebug(group.First()), "Validator");
    }
    
    private (int, string) GetDebug(ASTNode node)
    {
        return (-1, "This is not supported");
        
        // return (debugTable.GetLineIndex(node.DebugIndex) + 1, sourceFileTable[debugTable.GetFileId(node.DebugIndex)]);
    }
}