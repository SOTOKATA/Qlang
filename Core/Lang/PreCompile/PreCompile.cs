using System.Text.RegularExpressions;
using Qlang.Core.Lang.Compiler;
using Qlang.Core.Lang.Dynamic.Exceptions;
using Qlang.Core.LangDebug;

namespace Qlang.Core.Lang.PreCompile;

public static class PreCompile
{
    private static readonly HashSet<string> Included = new(StringComparer.OrdinalIgnoreCase);
    
    public static string IncludeFiles(string script, string fileName)
    {
        Logger.Log($"File: " + fileName, "IncludeFiles");
        // Find all lines with '{Keywords.IncludeKeyword} '
        var includeLines = script
            .Split('\n')
            .Select((line, index) => (Line: line.Trim(), LineNumber: index + 1))
            .Where(x => x.Line.StartsWith(Keywords.IncludeKeyword + " "))
            .ToArray();

        if (includeLines.Length > 0)
            Logger.Log(string.Join(", ", includeLines), Keywords.IncludeKeyword);

        List<string> files = [];

        foreach (var includeLine in includeLines)
        {
            var line = includeLine.Line.Replace($"{Keywords.IncludeKeyword} ", "").Replace("\"", "");
            var index = includeLine.LineNumber;

            string fullPath;

            if (line.StartsWith('$'))
            {
                var exePath = Path.GetDirectoryName(Environment.ProcessPath);

                if (exePath is null)
                    throw new QlangCompileException($"{Keywords.IncludeKeyword} '{line}' Error: Process path is not found", index, "PreCompile/IncludeFiles", fileName);
                
                fullPath = Path.Combine(exePath, line[1..]);
            }
            else
                fullPath = Path.Combine(Directory.GetCurrentDirectory(), line.StartsWith('$') ? line[1..] : line);

            fullPath = fullPath.Replace('\\', Path.DirectorySeparatorChar)
                                .Replace('/', Path.DirectorySeparatorChar);

            Logger.Log(fullPath, "Path");

            if (!Directory.Exists(fullPath) && !File.Exists(fullPath + ".ql"))
                throw new QlangCompileException($"{Keywords.IncludeKeyword}file or directory not found: {fullPath}", index, "PreCompile/IncludeFiles", fileName);

            if (!Directory.Exists(fullPath))
                fullPath += ".ql";

            script = script.Replace(includeLine.Line, "");

            if (!Included.Add(fullPath))
            {
                Logger.Warn($"{fullPath}", "Skipped (already included)");
                continue;
            }
            
            var content = "";

            if (File.Exists(fullPath) && fullPath.EndsWith(".ql"))
            {
                fileName = fullPath;
                content = $"#FILE \"{fullPath}\"\n" +  IncludeFiles(File.ReadAllText(fullPath), fileName);
            }
            else if (Directory.Exists(fullPath))
            {
                foreach (string file in Directory.GetFiles(fullPath).Where(file => Path.GetExtension(file) == ".ql"))
                {
                    Logger.Log(file, "Path");
                    if (!Included.Add(file))
                    {
                        Logger.Warn($"{fullPath}", "Skipped (already included)");
                        continue;
                    }

                    if (Path.GetExtension(file) == ".ql")
                    {
                        fileName = file;
                        content += $"#FILE \"{file}\"\n" + (IncludeFiles(File.ReadAllText(file), fileName) + "\n");
                    }
                }
            }
            else
            {
                throw new QlangCompileException($"File '{fullPath}' is not Qlang file type (.ql)", index, "PreCompile/IncludeFiles", fileName);
            }
            
            files.Add(content);
        }

        if (files.Count <= 0) 
            return script;
        
        return script + Environment.NewLine + string.Join(Environment.NewLine, files);

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
            string comment = match.Value;
        
            Logger.Log($"comment='{comment}'");
        
            return "";
        });
        
        Logger.Succ("Comments cleared successfully");
        return result;
    }

}