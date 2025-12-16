using System.Text;
using System.Text.RegularExpressions;
using Qlang.Core.Lang.Compiler;
using Qlang.Core.Lang.Dynamic.Exceptions;
using Qlang.Core.Lang.Interpreter.Native;
using Qlang.Core.LangDebug;

namespace Qlang.Core.Lang.PreCompile;

public static class PreCompile
{
    private static readonly HashSet<string> Included = new(StringComparer.OrdinalIgnoreCase);

    public static string IncludeFiles(string script, string fileName)
    {
        Logger.Log($"File: " + fileName, "IncludeFiles");

        var includeLines = script
            .Split('\n')
            .Select((line, index) => (Line: line.Trim(), LineNumber: index + 1))
            .Where(x => x.Line.StartsWith(Keywords.IncludeKeyword + " "))
            .ToArray();

        if (includeLines.Length > 0)
            Logger.Log(string.Join(", ", includeLines), Keywords.IncludeKeyword);

        List<string> includedContents = [];

        foreach ((var source, var index) in includeLines)
        {
            var line = source.Replace($"{Keywords.IncludeKeyword} ", "").Replace("\"", "");

            string fullPath;

            if (line.StartsWith('$'))
            {
                var exePath = Path.GetDirectoryName(Environment.ProcessPath);

                if (exePath is null)
                    throw new QlangCompileException(
                        $"{Keywords.IncludeKeyword} '{line}' Error: Process path is not found", index,
                        "PreCompile/IncludeFiles", fileName);

                fullPath = Path.Combine(exePath, line[1..]);
            }
            else
                fullPath = Path.Combine(Directory.GetCurrentDirectory(), line);

            fullPath = fullPath.Replace('\\', Path.DirectorySeparatorChar)
                .Replace('/', Path.DirectorySeparatorChar);

            Logger.Log(fullPath, "Path");

            var isDirectory = Directory.Exists(fullPath);
            var isFile = File.Exists(fullPath) || File.Exists(fullPath + ".ql");

            if (!isDirectory && !isFile)
                throw new QlangCompileException($"{Keywords.IncludeKeyword} file or directory not found: {fullPath}",
                    index, "PreCompile/IncludeFiles", fileName);

            script = script.Replace(source, "");

            if (isDirectory)
            {
                foreach (var file in Directory.GetFiles(fullPath, "*.ql", SearchOption.TopDirectoryOnly))
                {
                    Logger.Log(file, "Path");

                    if (!Included.Add(file))
                    {
                        Logger.Warn($"{file}", "Skipped (already included)");
                        continue;
                    }

                    var fileContent = File.ReadAllText(file);
                    var processedContent = IncludeFiles(fileContent, file);
                    includedContents.Add(processedContent);
                }
            }
            else
            {
                if (!fullPath.EndsWith(".ql"))
                    fullPath += ".ql";

                if (!fullPath.EndsWith(".ql"))
                    throw new QlangCompileException($"File '{fullPath}' is not Qlang file type (.ql)", index,
                        "PreCompile/IncludeFiles", fileName);

                if (!Included.Add(fullPath))
                {
                    Logger.Warn($"{fullPath}", "Skipped (already included)");
                    continue;
                }

                var fileContent = File.ReadAllText(fullPath);
                var processedContent = IncludeFiles(fileContent, fullPath);
                includedContents.Add(processedContent);
            }
        }

        StringBuilder result = new();

        foreach (var content in includedContents)
            result.AppendLine(content);

        result.AppendLine($"#FILE \"{fileName}\"");
        result.Append(script);

        return result.ToString();
    }

    private static readonly HashSet<string> IncludedNative = new(StringComparer.OrdinalIgnoreCase);

    public static (NativeFunctionRegistry register, string newScript) IncludeNativeFiles(string script, string fileName, NativeFunctionRegistry nativeFunctions)
    {
        Logger.Log($"File: " + fileName, "IncludeNativeFiles");

        var includeLines = script
            .Split('\n')
            .Select((line, index) => (Line: line.Trim(), LineNumber: index + 1))
            .Where(x => x.Line.StartsWith(Keywords.IncludeNativeKeyword + " "))
            .ToArray();

        if (includeLines.Length > 0)
            Logger.Log(string.Join(", ", includeLines), Keywords.IncludeNativeKeyword);

        foreach ((var source, var index) in includeLines)
        {
            var line = source.Replace($"{Keywords.IncludeNativeKeyword} ", "").Replace("\"", "");

            string fullPath;

            if (line.StartsWith('$'))
            {
                var exePath = Path.GetDirectoryName(Environment.ProcessPath);

                if (exePath is null)
                    throw new QlangCompileException(
                        $"{Keywords.IncludeNativeKeyword} '{line}' Error: Process path is not found", index,
                        "PreCompile/IncludeNativeFiles", fileName);

                fullPath = Path.Combine(exePath, line[1..]);
            }
            else
                fullPath = Path.Combine(Directory.GetCurrentDirectory(), line);

            fullPath = fullPath.Replace('\\', Path.DirectorySeparatorChar)
                .Replace('/', Path.DirectorySeparatorChar);

            Logger.Log(fullPath, "Path");

            var isDirectory = Directory.Exists(fullPath);
            var isFile = File.Exists(fullPath) || File.Exists(fullPath + ".dll");

            if (!isDirectory && !isFile)
                throw new QlangCompileException($"{Keywords.IncludeNativeKeyword} file or directory not found: {fullPath}",
                    index, "PreCompile/IncludeNativeFiles", fileName);

            script = script.Replace(source, "");

            if (isDirectory)
            {
                foreach (var file in Directory.GetFiles(fullPath, "*.dll", SearchOption.AllDirectories))
                {
                    Logger.Log(file, "Path");

                    if (!IncludedNative.Add(file))
                    {
                        Logger.Warn($"{file}", "Skipped (already included)");
                        continue;
                    }

                    nativeFunctions.LoadNativeLib(file);
                }
            }
            else
            {
                if (!fullPath.EndsWith(".dll"))
                    fullPath += ".dll";

                if (!fullPath.EndsWith(".dll"))
                    throw new QlangCompileException($"File '{fullPath}' is not Native file type (.dll)", index,
                        "PreCompile/IncludeNativeFiles", fileName);

                if (!IncludedNative.Add(fullPath))
                {
                    Logger.Warn($"{fullPath}", "Skipped (already included)");
                    continue;
                }

                nativeFunctions.LoadNativeLib(fullPath);
            }
        }

        return (nativeFunctions, script);
    }
    
    public static (string outScript, Dictionary<string, object> dictionary) ExtractNumbers(string script)
    {
        Logger.Warn($"Extract Numbers");
        Dictionary<string, object> numberDictionary = [];
        
        var numberCounter = 0;

        const string pattern =
            @"(?<!___[A-Z]+_)\b\d+(?:\.\d+)?(?:[eE][+-]?\d+)?\b(?!___)";

        var result = Regex.Replace(script, pattern, match => 
        {
            var numberValue = match.Value;
            var key = $"___NUMBER_{numberCounter}___";
            
            if (int.TryParse(numberValue, out var @int))
                numberDictionary[key] = @int;
            else if (numberValue.TryParseNumber(out var @double))
                numberDictionary[key] = @double;
            else throw new Exception($"Undefined type of value '{numberValue}'");
            
            numberCounter++;
            
            Logger.Warn($"key='{key}', value='{numberValue}'");
            
            return key;
        });
        
        Logger.Warn("Numbers extracted successfully");
        return (result, numberDictionary);
    }
    
    public static (string outScript, Dictionary<string, string> dictionary) ExtractStrings(string script)
    {
        Logger.Log($"Extract Strings");
        Dictionary<string, string> stringDictionary = [];
        
        var stringCounter = 0;
        
        const string pattern = """
                               "(?:[^"\\]|\\.)*"
                               """;
        
        var result = Regex.Replace(script, pattern, match => 
        {
            var stringValue = match.Value;
            var key = $"___STRING_{stringCounter}___";

            var value = stringValue.Substring(1, stringValue.Length - 2);
            
            value = value.Replace("\"", "\"\"");
            
            stringDictionary[key] = value;
            
            stringCounter++;
            
            Logger.Log($"key='{key}', value='{value}'");
            
            return key;
        });
        
        Logger.Succ("Strings extracted successfully");
        return (result, stringDictionary);
    }
    
    public static string ReturnFileStrings(string script, Dictionary<string, string> dictionary)
    {
        Logger.Log("Return File Strings");
    
        var result = Regex.Replace(script, @"^#FILE\s+(___STRING_\d+___)", match =>
        {
            var key = match.Groups[1].Value; // ___STRING_124___
        
            return dictionary.TryGetValue(key, out var path) 
                ? $"#FILE {path}" 
                : match.Value;
        }, RegexOptions.Multiline);
    
        Logger.Succ("Strings file returned successfully");
        return result;
    }

    public static string ClearComments(string script)
    {
        Logger.Succ("Clear Comments");
        const string pattern = @"//[^\r\n]*|/\*[\s\S]*?\*/";
        
        var result = Regex.Replace(script, pattern, match =>
        {
            var comment = match.Value;
        
            Logger.Log($"comment='{comment}'");
        
            return "";
        });
        
        Logger.Succ("Comments cleared successfully");
        return result;
    }

}