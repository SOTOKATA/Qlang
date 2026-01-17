namespace Core.AST;

public class ClassNode : ASTNode
{
    public string Name { get; set; }
    public string Extends { get; set; }
    public List<ASTNode> Body { get; set; }

    public override ASTNode Clone()
    {
        return new ClassNode
        {
            Name = Name, 
            Extends = Extends,
            Body = Body.Select(node => node.Clone()).ToList(),
            SourceFile =  SourceFile, 
            Line =  Line 
        };
    }

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                ClassNode:
                    Name: {Name}
                    Extends: {(Extends == "" ? "<not_exists>" : Extends)}
                    Body: [{string.Join(",\n", Body.Select(x => x.GetTree("\t\t")))}]
                """, indent);
    }
}