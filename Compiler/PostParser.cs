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
        var globalScopes = program.Statements.Where(x => x is ClassNode or FunctionNode or LineNode).ToList();

        // Remove all global scopes
        program.Statements.RemoveAll(x => x is ClassNode or FunctionNode or LineNode);

        var globalNamespace = new NamespaceNode
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

            var merged = new NamespaceNode
            {
                NameId = group.Key,
                Body = [],
                IsPrivate = group.First().IsPrivate
            };

            foreach (var ns in group)
                merged.Body.AddRange(ns.Body);

            body.RemoveAll(n => n is NamespaceNode ns && ns.NameId == group.Key);

            body.Add(merged);

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
        calls.AddRange(assignments.Where(x => x is { IsNew: false, IsPrivate: false }).Select(x => new CallNode
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
                    throw new QlangCompileException($"Namespace '{stringPoolTable[element!.NameId]}' was not found", GetDebug(element), "PostParser");
                
                lastNamespace = foundedNamespace;
            }

            var usingNamespaces = lastNamespace.Body.OfType<NamespaceNode>().ToList();
            var usingClasses = lastNamespace.Body.OfType<ClassNode>().ToList();
            var usingFunctions = lastNamespace.Body.OfType<FunctionNode>().ToList();
            var usingAssignments = lastNamespace.Body.OfType<LineNode>().Select(x => x.Content).OfType<AssignmentNode>().ToList();

            foreach (var callNode in calls)
            {
                if (processedCallNodes.Contains(callNode))
                    continue;
                
                var firstPathPart = callNode.Objects.First();
                
                switch (firstPathPart)
                {
                    case NamespacePointerNode pointer:
                        var found = usingNamespaces.FirstOrDefault(p => p.NameId == pointer.NameId);
                        
                        if (found is null || found.IsPrivate)
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
                        
                        if (foundObject is ClassNode { IsPrivate: true })
                            break;
                        
                        if (foundObject is AssignmentNode { IsPrivate: true })
                            break;
                        
                        callNode.Objects.InsertRange(0, @using.CallPath.Objects);
                        processedCallNodes.Add(callNode); 
                        break;
                    case FunctionPointerNode pointer:
                        // Find or function
                        var foundFunction = usingFunctions.FirstOrDefault(p => p.NameId == pointer.NameId && p.Parameters.Count == pointer.Arguments.Count);

                        if (foundFunction is null || foundFunction.IsPrivate)
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

        var objPath = new CallNode
        {
            Objects = [new ObjectPointerNode { NameId = stringPoolTable.Add(QlSystemClasses.ObjectClassName) }]
        };
        
        classNodes.ForEach(c =>
        {
            if (c.ExtendsPath == null && c.ExtendsPath?.Objects != objPath.Objects && c.NameId != stringPoolTable.Add(QlSystemClasses.ObjectClassName))
                c.ExtendsPath = objPath;
        });
        
        var namespaces = program.Statements.OfType<NamespaceNode>().ToList();
        for (var i = 0; i < namespaces.Count; i++)
            namespaces[i] = IncludeExtendsInNamespace(namespaces[i], namespaces);

        return program;
    }

    private NamespaceNode IncludeExtendsInNamespace(NamespaceNode namespaceNode, List<NamespaceNode> globalNamespaces)
    {
        var namespaces = namespaceNode.Body.OfType<NamespaceNode>().ToList();
        for (var i = 0; i < namespaces.Count; i++)
            namespaces[i] = IncludeExtendsInNamespace(namespaces[i], globalNamespaces);

        foreach (var @class in namespaceNode.Body.OfType<ClassNode>())
            ExtendClass(@class, globalNamespaces);

        return namespaceNode;
    }

    private ClassNode ExtendClass(ClassNode @class, List<NamespaceNode> globalNamespaces)
    {
        if (@class.ExtendsPath == null)
            return @class;
        
        ASTNode? currentPosition = null;
        foreach (var pathPart in @class.ExtendsPath.Objects)
        {
            switch (pathPart)
            {
                case FunctionPointerNode fp:
                    if (currentPosition is null)
                        throw new QlangCompileException("Undefined part of path: " + stringPoolTable[fp.NameId],
                            GetDebug(fp), "PostParser");

                    var globalNamespaceIndex = stringPoolTable.Add("~global");
                    var resultFp = globalNamespaces.FirstOrDefault(x => x.NameId == globalNamespaceIndex)?.Body.OfType<FunctionNode>().FirstOrDefault(f => f.NameId == fp.NameId);
                    
                    if (currentPosition is ClassNode c)
                        resultFp = c.Body.OfType<FunctionNode>().FirstOrDefault(f => f.NameId == fp.NameId);
                    
                    if (currentPosition is NamespaceNode ns)
                        resultFp = ns.Body.OfType<FunctionNode>().FirstOrDefault(f => f.NameId == fp.NameId);
                    
                    if (resultFp is null)
                        throw new QlangCompileException("Undefined part of path: " + stringPoolTable[fp.NameId],
                            GetDebug(fp), "PostParser");
                    
                    currentPosition = resultFp;
                    break;
                case ObjectPointerNode op:
                    ClassNode? resultOp = null;
                    if (currentPosition is not NamespaceNode @namespace)
                    {
                        var globalNIndex = stringPoolTable.Add("~global");
                        resultOp = globalNamespaces.FirstOrDefault(x => x.NameId == globalNIndex)?.Body.OfType<ClassNode>().FirstOrDefault(f => f.NameId == op.NameId);
                        currentPosition = resultOp;
                        
                        if (resultOp is null)
                            throw new QlangCompileException("Undefined part of path: " + stringPoolTable[op.NameId],
                            GetDebug(op), "PostParser");
                        break;
                    }

                    resultOp = @namespace.Body.OfType<ClassNode>()
                        .FirstOrDefault(x => x.NameId == op.NameId);

                    if (resultOp is null)
                        throw new QlangCompileException("Undefined part of path: " + stringPoolTable[op.NameId],
                            GetDebug(op), "PostParser");

                    currentPosition = resultOp;
                    break;
                case NamespacePointerNode on:
                    NamespaceNode? resultON;
                    
                    if (currentPosition is NamespaceNode node)
                    {
                        resultON = node.Body.OfType<NamespaceNode>()
                            .FirstOrDefault(x => x.NameId == on.NameId);

                        if (resultON is null)
                            throw new QlangCompileException("Undefined part of path: " + stringPoolTable[on.NameId],
                                GetDebug(on), "PostParser");
                        
                        currentPosition = resultON;
                        break;
                    }

                    resultON = globalNamespaces.FirstOrDefault(x => x.NameId == on.NameId);
                    
                    if (resultON is null)
                        throw new QlangCompileException("Undefined part of path: " + stringPoolTable[on.NameId],
                            GetDebug(on), "PostParser");
                    
                    currentPosition = resultON;
                    
                    break;
            }
        }
            
        if (currentPosition is not ClassNode extends)
            throw new QlangCompileException("Cannot extend not object.", GetDebug(@class),  "PostParser");
        
        // Object founded
        if (extends.ExtendsPath is not null)
            extends = ExtendClass(extends, globalNamespaces);

        extends.ExtendsPath = null;
        
        @class.Body = MergeBodies(extends.Body, @class.Body);
        
        return @class;
    }
    
    private static string GetNodeKey(ASTNode node)
    {
        if (node is LineNode ln)
        {
            if (ln.Content is AssignmentNode a)
            {
                var hash = "var:" + string.Join("_", a.Path.Select(astNode => astNode switch
                {
                    NamespacePointerNode n => "n" + n.NameId,
                    ObjectPointerNode o => "o" + o.NameId,
                    FunctionPointerNode f => "f" + f.NameId + "_" + f.Arguments.Count 
                }));
                // Console.WriteLine(hash);
                return hash;
            }
        }
        
        return node switch
        {
            FunctionNode fn => $"fn:{fn.NameId}_{fn.Parameters.Count}",
            CallNode call =>$"call:{string.Join(".", call.Objects)}",
            _ => throw new Exception($"Unsupported AST node: {node.GetType().Name}")
        };
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
            // return (debugTable.GetLineIndex(node.DebugIndex),
            //     table[debugTable.GetFileId(node.DebugIndex)]);
            return (-1, "This is not supported");
        }
        catch // Can be also if this is publish mode
        {
            return (-1, "");
        }
    }
}