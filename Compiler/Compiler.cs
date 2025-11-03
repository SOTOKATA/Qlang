using System.Text.RegularExpressions;
using Qlang.AST;
using Qlang.Dependencies;

namespace Qlang.Compiler;

public partial class Compiler
{
    public Compiler()
    {
        _parser = new(this);
        _lexer = new(this);
    }
    
    private string OriginalScript = "";
    
    private string OutputScript = "";
    
    public Dictionary<string, string> StringDictionary = [];

    private Parser _parser;
    private Lexer _lexer;
  
    public ProgramNode Compile(string script)
    {
        OriginalScript = script;
        
        OutputScript = ExtractStrings(OriginalScript);
        
        OutputScript = ClearComments(OutputScript);

        List<Token> tokens = _lexer.Lex(OutputScript);

        return _parser.Parse(tokens);
    }
    
    // Вставляет вместо "[content]" это: ___STRING_[counter]___
    private string ExtractStrings(string script)
    {
        StringDictionary.Clear();
        
        int stringCounter = 0;
        
        const string pattern = @"""(?:[^""\\]|\\.)*""";
        
        string result = Regex.Replace(script, pattern, match => 
        {
            string stringValue = match.Value;
            string key = $"___STRING_{stringCounter}___";
            
            StringDictionary[key] = stringValue.Substring(1, stringValue.Length - 2);
            
            stringCounter++;
            
            return key;
        });
        
        return result;
    }

    private static string ClearComments(string script)
    {
        const string pattern = @"//[^\r\n]*|/\*[\s\S]*?\*/";
        
        string result = Regex.Replace(script, pattern, "");
        
        return result;
    }
}