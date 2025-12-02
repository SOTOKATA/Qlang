using System.Text.RegularExpressions;
using Qlang.Compiler;
using Qlang.Dependencies;

namespace Qlang.PreCompile;

public static class PreCompile
{
    private static readonly HashSet<string> Included = new(StringComparer.OrdinalIgnoreCase);
    
    public static string IncludeFiles(string script)
    {
        // Find all lines with 'include '
        var includeLines = script
            .Split('\n')
            .Select(x => x.Trim())
            .Where(x => x.StartsWith(Keywords.IncludeKeyword + " "))
            .ToArray();

        if (includeLines.Length > 0)
            Logger.Logger.Log(string.Join(", ", includeLines), "Include");

        List<string> files = [];

        foreach (var includeLine in includeLines)
        {
            var line = includeLine.Replace("include ", "").Replace("\"", "");

            string fullPath;

            if (line.StartsWith('$'))
            {
                var exePath = Path.GetDirectoryName(Environment.ProcessPath);

                if (exePath is null)
                    throw new QlangCompileException($"Include '{line}' Error: Process path is not found", -1, "");
                
                fullPath = Path.Combine(exePath, line[1..]);
            }
            else
            {
                fullPath = Path.Combine(Directory.GetCurrentDirectory(), line.StartsWith('$') ? line[1..] : line);
            }

            

            fullPath = fullPath.Replace('\\', Path.DirectorySeparatorChar)
                                .Replace('/', Path.DirectorySeparatorChar);

            Logger.Logger.Log(fullPath, "Path");

            if (!Directory.Exists(fullPath) && !File.Exists(fullPath + ".ql"))
                throw new FileNotFoundException($"Include file or directory not found: {fullPath}");

            if (!Directory.Exists(fullPath))
                fullPath += ".ql";

            script = script.Replace(includeLine, "");

            if (!Included.Add(fullPath))
            {
                Logger.Logger.Warn($"{fullPath}", "Skipped (already included)");
                continue;
            }
            
            var content = "";

            if (fullPath.EndsWith(".ql"))
                content = File.ReadAllText(fullPath);
            else
            {
                foreach (string file in Directory.GetFiles(fullPath))
                {
                    Logger.Logger.Log(file, "Path");
                    if (!Included.Add(file))
                    {
                        Logger.Logger.Warn($"{fullPath}", "Skipped (already included)");
                        continue;
                    }
                    
                    if (Path.GetExtension(file) == ".ql") 
                        content += (File.ReadAllText(file) + "\n");
                }
            }
            
            var subScript = IncludeFiles(content);

            files.Add(subScript);
        }

        if (files.Count <= 0) 
            return script;
        
        return script + Environment.NewLine + string.Join(Environment.NewLine, files);

    }
    
    public static (string outScript, Dictionary<string, object> dictionary) ExtractNumbers(string script)
    {
        Logger.Logger.Warn($"Extract Numbers");
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
            
            Logger.Logger.Warn($"key='{key}', value='{numberValue}'");
            
            return key;
        });
        
        Logger.Logger.Warn("Numbers extracted successfully");
        return (result, numberDictionary);
    }
    
    public static (string outScript, Dictionary<string, string> dictionary) ExtractStrings(string script)
    {
        Logger.Logger.Log($"Extract Strings");
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
            
            Logger.Logger.Log($"key='{key}', value='{value}'");
            
            return key;
        });
        
        Logger.Logger.Succ("Strings extracted successfully");
        return (result, stringDictionary);
    }

    public static string ClearComments(string script)
    {
        Logger.Logger.Succ("Clear Comments");
        const string pattern = @"//[^\r\n]*|/\*[\s\S]*?\*/";
        
        var result = Regex.Replace(script, pattern, match =>
        {
            string comment = match.Value;
        
            Logger.Logger.Log($"comment='{comment}'");
        
            return "";
        });
        
        Logger.Logger.Succ("Comments cleared successfully");
        return result;
    }

}