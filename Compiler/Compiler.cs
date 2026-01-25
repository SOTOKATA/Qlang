using Core;
using Core.AST;

namespace Compiler;

public class Compiler
{
    public List<string> StringList = [];
    public List<double> NumberList = [];
    public List<QLIProgramLib> DllDependencies = [];
    public SourceFileTable SourceFileTable = new();

    public ProgramNode Compile(string fileName, string script)
    {
        string outputScript = PreCompile.IncludeFiles(script, fileName);
        
        (DllDependencies, outputScript) = PreCompile.IncludeNativeFolders(outputScript, fileName, []);
        
        outputScript = PreCompile.ClearComments(outputScript);
        
        (outputScript, StringList) = PreCompile.ExtractStrings(outputScript, []);
        (outputScript, StringList) = PreCompile.AddStringInterpolation(outputScript, StringList);
        (outputScript, StringList) = PreCompile.ExtractStrings(outputScript, StringList);
        
        (outputScript, NumberList) = PreCompile.ExtractNumbers(outputScript);

        outputScript = PreCompile.ReturnFileStrings(outputScript, StringList);

        var output = Lexer.Lex(fileName, outputScript);

        SourceFileTable = output.sourceFileTable;

        var programNode = new Parser().Parse(output.tokens, output.sourceFileTable);
        
        return programNode;
    }
}