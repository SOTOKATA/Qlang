using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Qlang.AST;

namespace Qlang.FileCompiler;

// To explain
public class FileCompiler
{
    public static void Compile()
    {
        
    }
public static void CreatePackedExe(string outputPath, byte[] programBytes)
    {
        // ------------- ШАБЛОННЫЕ ИСХОДНИКИ -------------
        // GeneratedMain.cs - точка входа итогового exe (извлекает ресурс и запускает интерпретатор)
        var generatedMain_cs = @"
using System;
using System.Reflection;
using System.IO;
using System.Text.Json;

public static class EntryPoint
{
    public static void Main(string[] args)
    {
        var asm = Assembly.GetExecutingAssembly();
        using var stream = asm.GetManifestResourceStream(""MyLang.Program.programdata"");
        if (stream == null) {
            Console.WriteLine(""Program data resource not found."");
            return;
        }
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        var bytes = ms.ToArray();

        // Deserialize ProgramNode (JSON) using same shape as in ProgramNode.cs
        var p = System.Text.Json.JsonSerializer.Deserialize<ProgramNode>(bytes);
        Interpreter.Run(p);
    }
}
";

        // Interpreter.cs - простой рантайм
        var interpreter_cs = @"
using System;
using System.Collections.Generic;

public static class Interpreter
{
    public static void Run(ProgramNode p)
    {
        Console.WriteLine($""[Interpreter] Program: {p.Name}"");
        if (p.Instructions == null) return;
        foreach (var i in p.Instructions)
        {
            Console.WriteLine($""[Interpreter] instr: {i}"");
        }
    }
}
";

        // ProgramNode.cs - определение класса и shape, совпадающий с тем, что сериализуем
        var programnode_cs = @"
using System.Collections.Generic;

public class ProgramNode
{
    public string Name { get; set; }
    public List<string> Instructions { get; set; }
}
";

        // ------------- Подготовка Roslyn-компиляции -------------
        var syntaxTrees = new[]
        {
            CSharpSyntaxTree.ParseText(generatedMain_cs),
            CSharpSyntaxTree.ParseText(interpreter_cs),
            CSharpSyntaxTree.ParseText(programnode_cs)
        };

        // Список необходимых ссылок (собираем пути к используемым сборкам)
        var references = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location), // System.Private.CoreLib / mscorlib
            MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Collections.Generic.List<>).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Text.Json.JsonSerializer).Assembly.Location),
            // иногда полезно явно добавить System.Runtime:
            // MetadataReference.CreateFromFile(typeof(System.Runtime.GCSettings).Assembly.Location)
        };

        // Создаём компиляцию
        var compilation = CSharpCompilation.Create(
            assemblyName: "PackedProgram",
            syntaxTrees: syntaxTrees,
            references: references,
            options: new CSharpCompilationOptions(OutputKind.ConsoleApplication)
        );

        // Встраиваем сериализованный ProgramNode как ресурс
        var resource = new ResourceDescription(
            "MyLang.Program.programdata", // имя ресурса — нужно такое же, как в GeneratedMain.cs
            () => new MemoryStream(programBytes),
            isPublic: true
        );

        // Emit в файл
        using var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
        var emitResult = compilation.Emit(fs, manifestResources: new[] { resource });

        if (!emitResult.Success)
        {
            Console.WriteLine("Compilation failed. Diagnostics:");
            foreach (var diag in emitResult.Diagnostics)
            {
                Console.WriteLine(diag.ToString());
            }
            throw new Exception("Failed to emit packed exe");
        }
    }
}