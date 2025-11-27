using Qlang.AST;
using Qlang.Dependencies;

namespace Qlang.Compiler;

public class Compiler
{
    private string _originalScript = "";
    
    private string _outputScript = "";
    
    public Dictionary<string, string> StringDictionary = [];
    public Dictionary<string, object> NumberDictionary = [];

    private readonly Parser _parser = new();
    private readonly Lexer _lexer = new();
  
    public ProgramNode Compile(string script)
    {
        _originalScript = script;

        FileLogger fl = new("Logs\\script.ql");
        fl.Log(_originalScript);
        
        Logger.Logger.SetLoggerPath(@"Logs\Debug\debug_pre_compile.log");
        
        Logger.Logger.Log("Include Files");
        _outputScript = PreCompile.PreCompile.IncludeFiles(_originalScript);
        Logger.Logger.Succ("All includes processed successfully.");
        
        _outputScript = PreCompile.PreCompile.ClearComments(_outputScript);
        
        (_outputScript, StringDictionary) = PreCompile.PreCompile.ExtractStrings(_outputScript);
        
        (_outputScript, NumberDictionary) = PreCompile.PreCompile.ExtractNumbers(_outputScript);
        
        fl.SetPath("Logs\\script_pre_compiled.ql");
        fl.Log(_outputScript);
        
        List<Token> tokens = _lexer.Lex(_outputScript);

        fl.SetPath("Logs\\script_tokenized.js");

        var line = "";
        int indent = 0;
        foreach (var token in tokens)
        {
            line += $"{token.TokenType}{(token.Value == "" ? "" : ($"({token.Value})"))} ";
            
            switch (token.TokenType)
            {
                case Tokens.LBrace:
                    indent++;
                    break;
                case Tokens.RBrace:
                    indent--;
                    break;
            }
            
            if (token.TokenType is Tokens.Semicolon or Tokens.Colon or Tokens.LBrace or Tokens.RBrace)
            {
                fl.Log(new string('\t', indent) + line);
                line = "";
            }
        }

        var programNode = _parser.Parse(tokens);
        
        fl.SetPath("Logs\\script_parsed.txt");
        
        fl.Log(programNode.GetTree());
        
        return programNode;
    }
}