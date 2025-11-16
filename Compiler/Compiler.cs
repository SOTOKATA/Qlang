using Qlang.AST;
using Qlang.Dependencies;

namespace Qlang.Compiler;

public class Compiler
{
    private string _originalScript = "";
    
    private string _outputScript = "";
    
    public Dictionary<string, string> StringDictionary = [];
    public Dictionary<string, string> NumberDictionary = [];

    private readonly Parser _parser = new();
    private readonly Lexer _lexer = new();
  
    public ProgramNode Compile(string script)
    {
        _originalScript = script;

        FileLogger fl = new("Logs\\script.txt");
        fl.Log(_originalScript);

        _outputScript = PreCompile.PreCompile.IncludeFiles(_originalScript);

        (_outputScript, StringDictionary) = PreCompile.PreCompile.ExtractStrings(_outputScript);
        
        (_outputScript, NumberDictionary) = PreCompile.PreCompile.ExtractNumbers(_outputScript);

        _outputScript = PreCompile.PreCompile.ClearComments(_outputScript);

        fl.SetPath("Logs\\script_pre_compiled.ql");
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