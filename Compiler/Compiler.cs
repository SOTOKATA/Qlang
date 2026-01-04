using Core.AST;
using Core.Debug;
using Core.Native;
using Core.NativeLib.SystemLib;

namespace Compiler;

public class Compiler
{
    private string _originalScript = "";
    
    private string _outputScript = "";
    
    public Dictionary<string, string> StringDictionary = [];
    public Dictionary<string, object> NumberDictionary = [];
    public NativeFunctionRegistry NativeFunctions = new();

    private readonly Parser _parser = new();

    public ProgramNode Compile(string fileName, string script)
    {
        _originalScript = script; //"include \"$lib/base/datatypes\"" + Environment.NewLine + 

        FileLogger fl = new("Logs\\script.ql");
        fl.Log(_originalScript);
        
        Logger.SetLoggerPath(@"Logs\Debug\debug_pre_compile.log");

        Logger.Log("Include Files");
        _outputScript = PreCompile.IncludeFiles(_originalScript, fileName);
        Logger.Succ("All includes processed successfully.");
        
        Logger.Log("Include Native Files");
        (NativeFunctions, _outputScript) = PreCompile.IncludeNativeFiles(_outputScript, fileName, NativeFunctions);
        NativeFunctions.RegisterLib(new ConsoleLib());
        Logger.Succ("All native includes processed successfully.");
        
        _outputScript = PreCompile.ClearComments(_outputScript);
        
        (_outputScript, StringDictionary) = PreCompile.ExtractStrings(_outputScript);
        
        (_outputScript, NumberDictionary) = PreCompile.ExtractNumbers(_outputScript);

        _outputScript = PreCompile.ReturnFileStrings(_outputScript, StringDictionary);
        
        fl.SetPath("Logs\\script_pre_compiled.ql");
        fl.Log(_outputScript);

        List<Token> tokens;

        tokens = Lexer.Lex(fileName, _outputScript);

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
                fl.Log(new string('\t', (indent < 0 ? 0 : indent)) + line);
                line = "";
            }
        }

        ProgramNode programNode;
            
        try
        {
            programNode = _parser.Parse(tokens);
        }
        catch
        {
            throw;
        }

        fl.SetPath("Logs\\script_parsed.txt");
        
        fl.Log(programNode.GetTree());
        
        return programNode;
    }
}