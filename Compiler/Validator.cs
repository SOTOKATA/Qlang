using Core.AST;
using Core.Exceptions;
using Core.Tables;

namespace Compiler;

public class Validator(SourceFileTable? sourceFileTable, DebugTable? debugTable, StringPoolTable stringPoolTable)
{
    
    public void CheckValidate(ProgramNode program)
    {
        CheckDuplicate(program);
    }
    
    
    private void CheckDuplicate(ProgramNode program)
    {
        List<ClassNode> classes = [];
        
        var namespaces = program.Statements.OfType<NamespaceNode>().ToList();

        foreach (var @namespace in program.Statements.OfType<NamespaceNode>())
            classes.AddRange(Parser.GetClassesFromNamespaceRecursively(@namespace));
        
        foreach (var @namespace in namespaces.ToList())
            namespaces.AddRange(Parser.GetNamespacesFromNamespaceRecursively(@namespace));

        CheckDuplicateClasses(classes);

        foreach (var classNode in classes)
        {
            CheckDuplicateFunctions(classNode);
            CheckDuplicateAssignments(classNode);
        }

        foreach (var @namespace in namespaces)
        {
            CheckDuplicateFunctions(@namespace);
            CheckDuplicateAssignments(@namespace);
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

    private void CheckDuplicateFunctions(object obj)
    {
        List<FunctionNode> functions = [];
        var objectName = "";
        
        if (obj is ClassNode @class)
        {
            functions = @class.Body
                .OfType<FunctionNode>()
                .ToList();
            objectName = stringPoolTable[@class.NameId];
        }
        else if (obj is NamespaceNode @namespace)
        {
            functions = @namespace.Body
                .OfType<FunctionNode>()
                .ToList();
            objectName = stringPoolTable[@namespace.NameId];
        }

        var duplicateGroups = functions
            .GroupBy(f => new {
                Name = f.NameId,
                ParamCount = f.Parameters.Count,
            })
            .Where(g => g.Count() > 1);

        foreach (var group in duplicateGroups)
            throw new QlangCompileException(
                $"Duplicate function found: '{stringPoolTable[group.Key.Name]}' in '{objectName}'",
                GetDebug(group.First()),
                "Validator"
            );
    }
    
    private void CheckDuplicateAssignments(object obj)
    {
        var objectName = "";
        List<AssignmentNode> assignments = [];

        if (obj is ClassNode @class)
        {
            assignments = @class.Body
                .OfType<LineNode>().Select(x => (AssignmentNode)x.Content!)
                .ToList();
            objectName = stringPoolTable[@class.NameId];
        } else if (obj is NamespaceNode @namespace)
        {
            assignments = @namespace.Body
                .OfType<LineNode>().Select(x => (AssignmentNode)x.Content!)
                .ToList();
            objectName = stringPoolTable[@namespace.NameId];
        }

        if (assignments.Count == 0)
            return;
        
        var duplicateGroups = assignments
            .GroupBy(f => new {
                VariableNameId = f.GetLastNameId()
            })
            .Where(g => g.Count() > 1);

        foreach (var group in duplicateGroups)
            throw new QlangCompileException($"Duplicate assignment found: '{stringPoolTable[group.Key.VariableNameId]}' in class '{objectName}'", GetDebug(group.First()), "Validator");
    }
    
    private (int, string) GetDebug(ASTNode node)
    {
        return (-1, "This is not supported");
        
        // return (debugTable.GetLineIndex(node.DebugIndex) + 1, sourceFileTable[debugTable.GetFileId(node.DebugIndex)]);
    }
}