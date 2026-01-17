namespace Core;

// List of system classes
public static class QlSystemClasses
{
    public const string StringClassName = "String";
    public const string ObjectClassName = "Object";
    public const string ArrayClassName = "Array";

    public static List<string> GetClassNames() => [ StringClassName, ObjectClassName, ArrayClassName ];
}