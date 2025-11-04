using System.Text.RegularExpressions;
using Qlang.AST;
using Qlang.Dependencies;

namespace Qlang.Compiler;

public partial class Compiler
{
    public Compiler()
    {
        _parser = new();
        _lexer = new(this);
    }
    
    private string _originalScript = "";
    
    private string _outputScript = "";
    
    public readonly Dictionary<string, string> StringDictionary = [];

    private readonly Parser _parser;
    private readonly Lexer _lexer;
  
    public ProgramNode Compile(string script)
    {
        FileLogger fl = new("Logs\\script.txt");
        
        _originalScript = script;
        
        fl.Log(_originalScript);
        
        _outputScript = ExtractStrings(_originalScript);
        
        // Console.WriteLine($"Code (ExtractStrings): \n{_outputScript}\n");
        
        fl.SetPath("Logs\\script_pre_compiled.txt");
        
        _outputScript = ClearComments(_outputScript);

        // Console.WriteLine($"Code (ClearComments): \n{_outputScript}\n");
        
        fl.Log(_outputScript);
        
        List<Token> tokens = _lexer.Lex(_outputScript);

        
        fl.SetPath("Logs\\script_tokenized.txt");

        string line = "";
        foreach (Token token in tokens)
        {
            line += $"{token.TokenType}{(token.Value == "" ? "" : ($"({token.Value})"))} ";

            switch (token.TokenType)
            {
                case Tokens.Indent or Tokens.Dedent:
                    fl.Log(line);
                
                    line = token.TokenType.ToString();
                
                    fl.Log(line);
                    break;
                case Tokens.NewLine:
                    fl.Log(line);
                    line = "";
                    break;
            }
        }

        ProgramNode programNode = _parser.Parse(tokens);
        
        fl.SetPath("Logs\\script_parsed.txt");
        
        fl.Log(string.Join(' ', programNode.Statements));
        
        return programNode;
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