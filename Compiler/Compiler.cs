using System.Diagnostics;
using Core;
using Core.AST;
using Core.Tables;

namespace Compiler;

public class Compiler
{
    public List<double> NumberList = [];
    public List<QLIProgramLib> DllDependencies = [];
    public SourceFileTable? SourceFileTable = new();
    public DebugTable? DebugTable = new();
    public StringPoolTable StringPoolTable = new();

    public ProgramNode Compile(string fileName, string script, bool isPublish)
    {
        var stopWatch = Stopwatch.StartNew();
        Console.Write("Importing...");

        (var outputScript, DllDependencies) = PreCompile.IncludeFiles(script, fileName, []);
        Console.WriteLine(" success.");
        
        outputScript = PreCompile.ClearComments(outputScript);
        
        (outputScript, StringPoolTable) = PreCompile.ExtractStrings(outputScript, new StringPoolTable());
        (outputScript, StringPoolTable) = PreCompile.AddStringInterpolation(outputScript, StringPoolTable);
        (outputScript, StringPoolTable) = PreCompile.ExtractStrings(outputScript, StringPoolTable);
        
        (outputScript, NumberList) = PreCompile.ExtractNumbers(outputScript);

        outputScript = PreCompile.ReturnFileStrings(outputScript, StringPoolTable);
        Console.WriteLine("Compiling " + outputScript.Split('\n').Length + " lines.");
        
        Console.WriteLine(outputScript);
        
        Console.Write("Lexing...");
        var output = Lexer.Lex(fileName, outputScript, isPublish);
        Console.WriteLine(" success.");

        SourceFileTable = output.sourceFileTable;
        DebugTable = output.debugTable;

        Console.Write("Parsing...");
        (var programNode, StringPoolTable) = new Parser(output.sourceFileTable, output.debugTable, StringPoolTable).Parse(output.tokens);
        Console.WriteLine(" success.");
        
        stopWatch.Stop();
        
        Console.WriteLine("\nCompiling time: " + stopWatch.Elapsed.Milliseconds + " ms.\n");
        
        return programNode;
    }
}