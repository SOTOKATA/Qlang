using Qlang.AST;
using Qlang.Dependencies;

namespace Qlang.Compiler;

public class Compiler
{
    public Compiler()
    {
        _parser = new Parser();
        _lexer = new Lexer(this);
    }
    
    private string _originalScript = "";
    
    private string _outputScript = "";
    
    public Dictionary<string, string> StringDictionary = [];

    private readonly Parser _parser;
    private readonly Lexer _lexer;
  
    public ProgramNode Compile(string script)
    {
        _originalScript = script;
        
        FileLogger fl = new("Logs\\script.txt");
        fl.Log(_originalScript);

        _outputScript = PreCompile.IncludeFiles(script);
        
        (_outputScript, StringDictionary) = PreCompile.ExtractStrings(_originalScript);
        
        // Console.WriteLine($"Code (ExtractStrings): \n{_outputScript}\n");
        
        
        _outputScript = PreCompile.ClearComments(_outputScript);

        // Console.WriteLine($"Code (ClearComments): \n{_outputScript}\n");
        
        fl.SetPath("Logs\\script_pre_compiled.txt");
        fl.Log(_outputScript);
        
        List<Token> tokens = _lexer.Lex(_outputScript);

        fl.SetPath("Logs\\script_tokenized.txt");

        var line = "";
        foreach (var token in tokens)
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

        var programNode = _parser.Parse(tokens);
        
        fl.SetPath("Logs\\script_parsed.txt");
        
        fl.Log(programNode.GetTree());
        
        return programNode;
    }
}