namespace Core;

public static class Keywords
{
    public static string FunctionDeclaration => "function";
    public static string ShortFunctionDeclaration => "fn";
    public static string ClassDeclaration => "class";
    public static string NamespaceDeclaration => "namespace";
    public static string VariableDeclaration => "let";
    public static string ConstVariableDeclaration => "const";
    public static string FieldKeyword => "field";
    
    public static string ThisKeyword => "this";
    
    public static string NullKeyword => "null";
    
    public static string SwitchBlock => "switch";
    public static string CaseKeyword => "case";
    public static string DefaultKeyword => "default";
    
    public static string AwaitKeyword => "await";
    public static string AsyncKeyword => "async";
    public static string ParallelKeyword => "parallel";
    
    public static string IfBlock => "if";
    public static string ElseBlock => "else";
    public static string WhileBlock => "while";
    public static string DoWhileBlock => "do_while";
    public static string ForBlock => "for";
    public static string TryKeyword => "try";
    public static string CatchKeyword => "catch";
    public static string FinallyKeyword => "finally";
    
    public static string ReturnKeyword => "return";
    public static string BreakKeyword => "break";
    public static string ContinueKeyword => "continue";
    
    public static string TrueKeyword => "true";
    public static string FalseKeyword => "false";
    public static string TypeEqualityKeyword => "is";
    public static string TypeNotEqualityKeyword => "not";

    public static string ImportKeyword => "import";
    public static string ImportNativeKeyword => "native";
    public static string UsingKeyword => "using";
    
    public static string CreateClassInstanceKeyword => "new";
    
    public static string ExtendsKeyword => "extends";
  
    public static string PrivateModificator => "private";
    
    public static List<string> GetKeywords() =>
    [
        FunctionDeclaration, ClassDeclaration, VariableDeclaration, IfBlock, ElseBlock, WhileBlock, DoWhileBlock, 
        ForBlock, ReturnKeyword, TrueKeyword, FalseKeyword, ImportKeyword, PrivateModificator, 
        ConstVariableDeclaration, ContinueKeyword, BreakKeyword, NullKeyword, ThisKeyword, ExtendsKeyword, 
        SwitchBlock, CaseKeyword, DefaultKeyword, NamespaceDeclaration,
        UsingKeyword, TryKeyword, CatchKeyword, FinallyKeyword, ShortFunctionDeclaration,
        CreateClassInstanceKeyword, AwaitKeyword, AsyncKeyword, ParallelKeyword,
        TypeEqualityKeyword, TypeNotEqualityKeyword, FieldKeyword
    ];
}