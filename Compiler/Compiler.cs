using Core;
using Core.AST;

namespace Compiler;

public class Compiler
{
    public List<string> StringList = [];
    public List<double> NumberList = [];
    public List<QLIProgramLib> DllDependencies = [];
    public SourceFileTable? SourceFileTable = new();
    public DebugTable? DebugTable = new();

    public ProgramNode Compile(string fileName, string script, bool isPublish)
    {
        (var outputScript, DllDependencies) = PreCompile.IncludeFiles(script, fileName, []);
        
        outputScript = PreCompile.ClearComments(outputScript);
        
        (outputScript, StringList) = PreCompile.ExtractStrings(outputScript, []);
        (outputScript, StringList) = PreCompile.AddStringInterpolation(outputScript, StringList);
        (outputScript, StringList) = PreCompile.ExtractStrings(outputScript, StringList);
        
        (outputScript, NumberList) = PreCompile.ExtractNumbers(outputScript);

        outputScript = PreCompile.ReturnFileStrings(outputScript, StringList);

        var output = Lexer.Lex(fileName, outputScript, isPublish);

        SourceFileTable = output.sourceFileTable;
        DebugTable = output.debugTable;

        var programNode = new Parser().Parse(output.tokens, output.sourceFileTable, output.debugTable);
        
        return programNode;
    }
}