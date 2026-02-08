using Core.AST;
using Core.Tables;
using MessagePack;

namespace Core.Dynamic;

[MessagePackObject]
public class Variable(string name, object? value, bool isStatic, bool isPrivate, bool isConst, CallNode? type = null)
{
    public Variable() : this("", null, false, false, false) {}
    public override string? ToString()
    {
        return Value?.ToString();       
    }

    public static Variable FromAssignmentNode(AssignmentNode assignNode, object? value, StringPoolTable stringPoolTable)
    {
        return new Variable(stringPoolTable[assignNode.GetLastNameId()], value, assignNode.IsStatic, assignNode.IsPrivate, assignNode.IsConst, assignNode.Type);
    }

    [Key(1)]
    public string Name { get; set; } = name;
    [Key(2)]
    public object? Value { get; set; } = value;
    [Key(3)]
    public CallNode? Type { get; set; } = type;
    [Key(4)]
    public bool IsStatic { get; set; } = isStatic;
    [Key(5)]
    public bool IsConst { get; set; } = isConst;
    [Key(6)]
    public bool IsPrivate { get; set; } = isPrivate;
}