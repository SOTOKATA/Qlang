namespace Qlang.Core.Lang.Compiler;

public static class Keywords
{
    public static string FunctionDeclaration => "function";
    public static string ClassDeclaration => "class";
    public static string VariableDeclaration => "let";
    
    public static string ThisKeyword => "this";
    
    public static string NullKeyword => "null";
    
    public static string SwitchBlock => "switch";
    public static string CaseKeyword => "case";
    public static string DefaultKeyword => "default";
    
    public static string IfBlock => "if";
    public static string ElseBlock => "else";
    public static string WhileBlock => "while";
    public static string DoWhileBlock => "do_while";
    public static string ForBlock => "for";
    
    public static string ReturnKeyword => "return";
    public static string BreakKeyword => "break";
    public static string ContinueKeyword => "continue";
    
    public static string TrueKeyword => "true";
    public static string FalseKeyword => "false";

    public static string IncludeKeyword => "include";
    public static string IncludeNativeKeyword => "native";
    
    public static string ExtendsKeyword => "extends";
  
    public static string PrivateModificator => "private";
    public static string StaticModificator => "static";
    public static string ConstVariableDeclaration => "const";
    
    public static List<string> GetKeywords() =>
    [
        FunctionDeclaration, ClassDeclaration, VariableDeclaration, IfBlock, ElseBlock, WhileBlock, DoWhileBlock, 
        ForBlock, ReturnKeyword, TrueKeyword, FalseKeyword, IncludeKeyword, PrivateModificator, 
        StaticModificator, ConstVariableDeclaration, ContinueKeyword, BreakKeyword, NullKeyword, ThisKeyword, ExtendsKeyword, SwitchBlock, CaseKeyword, DefaultKeyword
    ];
}