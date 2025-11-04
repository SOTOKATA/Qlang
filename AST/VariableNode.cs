namespace Qlang.AST;

public class VariableNode : ASTNode
{
    public string Name { get; set; }
    
    public DataTypes Type { get; set; }
}

public enum DataTypes
{
    Number,
    String,
}