namespace Qlang.Compiler;

public class Keywords
{
    public static string FunctionDeclaration => "function";
    public static string ClassDeclaration => "class";
    public static string VariableDeclaration => "let";
    
    public static string IfBlock => "if";
    public static string ElseBlock => "else";
    public static string WhileBlock => "while";
    public static string DoWhileBlock => "do_while";
    public static string ForBlock => "for";
    
    public static string ReturnKeyword => "return";
    
    public static string TrueKeyword => "true";
    public static string FalseKeyword => "false";

    public static string IncludeKeyword => "include";
  
    public static string PrivateModificator => "private";
    public static string StaticModificator => "static";
    public static string ConstModificator => "const";
    
    public static List<string> GetKeywords() =>
    [
        FunctionDeclaration, ClassDeclaration, VariableDeclaration, IfBlock, ElseBlock, WhileBlock, DoWhileBlock, 
        ForBlock, ReturnKeyword, TrueKeyword, FalseKeyword, IncludeKeyword, PrivateModificator, 
        StaticModificator, ConstModificator
    ];
}