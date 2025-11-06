namespace Qlang.AST;

public class ASTGetTreeBuilder
{
    public static string Build(string astName, List<object> nodes, string indent = "")
    {
        string result = $"{indent}@{astName}:";

        foreach (object node in nodes)
        {
            string toWrite = ""; //$"{indent}" + "|---"
            
            string subIndent = "[--]";
            
            toWrite += node switch
            {
                ASTNode astNode => BuildASTNode(astNode, subIndent),
                List<ASTNode> astNodes => astNodes.Aggregate(toWrite,
                    (current, astNode) => current + "\n" + BuildASTNode(astNode, subIndent)),
                List<string> list => list.Aggregate(toWrite,
                    (current, listItem) => current + "\n" + $"{subIndent}{listItem}"),
                List<object> list => list.Aggregate(toWrite,
                    (current, listItem) => current + "\n" + subIndent + listItem),
                var _ => subIndent + node
            };

            result += "\n" + toWrite;
        }
//nodes.Aggregate(result, (current, node) => current)
        return result;
    }

    private static string BuildASTNode(ASTNode astNode, string indent = "")
    {
        return astNode.GetTree(indent);
    }
}