using Core;
using Core.AST;
using Core.Exceptions;
using Core.Tables;

namespace Compiler;

public class PostParser(SourceFileTable table, DebugTable debugTable, StringPoolTable stringPoolTable)
{
    /// <summary>
    /// Creates global namespace for global scopes
    /// </summary>
    /// <param name="program">program where need to create global namespace</param>
    /// <returns>program with global namespace</returns>
    public ProgramNode CreateGlobalNamespace(ProgramNode program)
    {
        // Get all global scopes like Class, Function or Assignment
        var globalScopes = program.Statements.Where(x => x is ClassNode or FunctionNode or AssignmentNode).ToList();

        // Remove all global scopes
        program.Statements.RemoveAll(x => x is ClassNode or FunctionNode or AssignmentNode);

        var globalNamespace = new NamespaceNode(globalScopes.FirstOrDefault()?.DebugIndex ?? -1)
        {
            NameId = stringPoolTable.Add("~global"),
            IsPrivate = false
        };
            
        globalNamespace.Body.AddRange(globalScopes);

        program.Statements.Add(globalNamespace);
        
        return program;
    }
    
    // Will merge namespaces with same name (partial namespaces)
    public void MergeNamespaces(List<ASTNode> body)
    {
        var namespaces = body.OfType<NamespaceNode>()
            .GroupBy(n => n.NameId)
            .ToList();

        foreach (var group in namespaces)
        {
            if (group.Count() == 1)
            {
                // всё равно надо рекурсивно
                
                MergeNamespaces(group.First().Body);
                continue;
            }

            var merged = new NamespaceNode(group.First().DebugIndex)
            {
                NameId = group.Key,
                Body = [],
                IsPrivate = group.First().IsPrivate
            };

            foreach (var ns in group)
                merged.Body.AddRange(ns.Body);

            // удалить старые
            body.RemoveAll(n => n is NamespaceNode ns && ns.NameId == group.Key);

            // добавить новый
            body.Add(merged);

            // рекурсивно мержим вложенные
            MergeNamespaces(merged.Body);
        }
    }

    /// <summary>
    /// Find all usings and include to paths
    /// </summary>
    /// <param name="program">program where need to include usings</param>
    /// <param name="calls">all CallPath in program</param>
    /// <param name="assignments">all AssignmentNode in program</param>
    /// <returns>program with included usings</returns>
    /// <exception cref="QlangCompileException">Will throw exception if using was not found in namespaces</exception>
    public ProgramNode IncludeUsings(ProgramNode program, List<CallNode> calls, List<AssignmentNode> assignments)
    {
        calls.AddRange(assignments.Where(x => x is { IsNew: false, IsPrivate: false }).Select(x => new CallNode(x.DebugIndex)
        {
            Objects = x.Path
        }));
        
        // Get all usings and remove from list
        var usings= program.Statements.OfType<UsingNode>().ToList();
        program.Statements.RemoveAll(s => s is UsingNode);

        // Remove all paths what are from usings
        var toRemove = new List<CallNode>();
        foreach (var call in calls)
        {
            foreach (var @using in usings)
            {
                var usingNames = @using.CallPath.Objects.Select((x) =>
                {
                    return x switch
                    {
                        NamespacePointerNode p => p.NameId,
                        ObjectPointerNode p => p.NameId,
                        FunctionPointerNode p => p.NameId,
                    };
                }).ToList();
                
                var callNames = call.Objects.Select((x) =>
                {
                    return x switch
                    {
                        NamespacePointerNode p => p.NameId,
                        ObjectPointerNode p => p.NameId,
                        FunctionPointerNode p => p.NameId,
                    };
                }).ToList();

                if (usingNames.SequenceEqual(callNames))
                    toRemove.Add(call);
            }
        }

        foreach (var call in toRemove)
            calls.Remove(call);

        // Get global namespaces
        List<NamespaceNode> globalNamespaces = program.Statements.OfType<NamespaceNode>().ToList();

        HashSet<CallNode> processedCallNodes = [];
        foreach (var @using in usings)
        {
            // Get all namespaces what is used in using path (std::inter::lock)
            var pathToNamespace = @using.CallPath;

            // Found namespace from using
            NamespaceNode? lastNamespace = null;
            foreach (var element in pathToNamespace.Objects.Cast<NamespacePointerNode?>())
            {
                var foundedNamespace =
                    // Found in global list
                    lastNamespace is null ? globalNamespaces.FirstOrDefault(x => x.NameId == element?.NameId) :
                    // Found in subnamespaces
                    lastNamespace.Body.OfType<NamespaceNode>().FirstOrDefault(x => x.NameId == element?.NameId);
                
                if (foundedNamespace is null)
                    throw new QlangCompileException($"Namespace '{element?.NameId}' was not found", GetDebug(element), "PostParser");
                
                lastNamespace = foundedNamespace;
            }
            
            var usingNamespaces = lastNamespace.Body.OfType<NamespaceNode>().ToList();
            var usingClasses = lastNamespace.Body.OfType<ClassNode>().ToList();
            var usingFunctions = lastNamespace.Body.OfType<FunctionNode>().ToList();
            var usingAssignments = lastNamespace.Body.OfType<AssignmentNode>().ToList();

            foreach (var callNode in calls)
            {
                if (processedCallNodes.Contains(callNode))
                    continue;

                var firstPathPart = callNode.Objects.First();
                
                switch (firstPathPart)
                {
                    case NamespacePointerNode pointer:
                        var found = usingNamespaces.FirstOrDefault(p => p.NameId == pointer.NameId);
                        
                        if (found is null)
                            break;
                        
                        callNode.Objects.InsertRange(0, @using.CallPath.Objects);
                
                        processedCallNodes.Add(callNode);
                        break;
                    case ObjectPointerNode pointer:
                        // Find or class or assignment (var)
                        var foundObject = usingClasses.FirstOrDefault(p => p.NameId == pointer.NameId) ?? 
                                          (ASTNode?)usingAssignments.FirstOrDefault(a => a.Path.Exists(pathPart =>
                                          {
                                              return pathPart switch
                                              {
                                                  NamespacePointerNode p => p.NameId == pointer.NameId,
                                                  ObjectPointerNode p => p.NameId == pointer.NameId,
                                                  FunctionPointerNode p => p.NameId == pointer.NameId,
                                              };
                                          }));

                        if (foundObject is null)
                            break;
                        
                        callNode.Objects.InsertRange(0, @using.CallPath.Objects);
                        processedCallNodes.Add(callNode); 
                        break;
                    case FunctionNode pointer:
                        // Find or function
                        var foundFunction = usingFunctions.FirstOrDefault(p => p.NameId == pointer.NameId && p.Parameters.Count == pointer.Parameters.Count);

                        if (foundFunction is null)
                            break;
                        
                        callNode.Objects.InsertRange(0, @using.CallPath.Objects);
                        processedCallNodes.Add(callNode);
                        break;
                }
            }
        }

        return program;
    }  
    
    /// <summary>
    /// Includes all extends for classes
    /// </summary>
    /// <param name="program">program where need to include extends</param>
    /// <returns>program with included extends</returns>
    public ProgramNode IncludeExtends(ProgramNode program)
    {
        var classNodes = new List<ClassNode>();

        foreach (var @namespace in program.Statements.OfType<NamespaceNode>())
            classNodes.AddRange(Parser.GetClassesFromNamespaceRecursively(@namespace));

        var objIndex = stringPoolTable.Add(QlSystemClasses.ObjectClassName);
        classNodes.ForEach(c =>
        {
            if (string.IsNullOrEmpty(stringPoolTable[c.ExtendsId]) && c.ExtendsId != objIndex && c.NameId != objIndex)
                c.ExtendsId = objIndex;
        });
            

        foreach (var cls in classNodes)
        {
            ResolveClass(
                cls,
                classNodes,
                []
            );
        }

        return program;
    }
    
    private static string GetNodeKey(ASTNode node)
    {
        return node switch
        {
            FunctionNode fn => $"fn:{fn.NameId}_{fn.Parameters.Count}",
            // AssignmentNode an when an.GetLastNameId() != -1 => $"var:{an.GetLastNameId()}",
            AssignmentNode an => $"var:{an.Path}",
            CallNode call =>$"call:{string.Join(".", call.Objects)}",
            _ => throw new Exception($"Unsupported AST node: {node.GetType().Name}")
        };
    }
    
    private void ResolveClass(
        ClassNode cls,
        List<ClassNode> allClasses,
        HashSet<int> resolving)
    {
        if (stringPoolTable[cls.ExtendsId] == "")
            return;

        if (!resolving.Add(cls.NameId))
            throw new QlangCompileException(
                $"Cyclic inheritance detected: {stringPoolTable[cls.NameId]}",
                GetDebug(cls),
                "PostParser");

        var parent = allClasses.FirstOrDefault(c => c.NameId == cls.ExtendsId);

        if (parent == null)
            throw new QlangCompileException(
                $"Extended class '{stringPoolTable[cls.ExtendsId]}' is not found",
                GetDebug(cls),
                "PostParser");

        ResolveClass(parent, allClasses, resolving);

        cls.Body = MergeBodies(parent.Body, cls.Body);

        cls.ExtendsId = stringPoolTable.Add("");

        resolving.Remove(cls.NameId);
    }

    
    private static List<ASTNode> MergeBodies(
        List<ASTNode> parentBody,
        List<ASTNode> childBody)
    {
        var map = new Dictionary<string, ASTNode>();

        foreach (var node in parentBody)
            map[GetNodeKey(node)] = node;

        foreach (var node in childBody)
            map[GetNodeKey(node)] = node;

        return map.Values.ToList();
    }
    
    private (int, string) GetDebug(ASTNode node)
    {
        try
        {
            return (debugTable.GetLineIndex(node.DebugIndex),
                table[debugTable.GetFileId(node.DebugIndex)]);
        }
        catch (Exception ex)
        {
            // Console.WriteLine(ex);
            return (-1, "");
        }
    }
}