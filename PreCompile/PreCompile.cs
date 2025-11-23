using System.Text.RegularExpressions;
using Qlang.Compiler;

namespace Qlang.PreCompile;

public static class PreCompile
{
    private static readonly HashSet<string> Included = new(StringComparer.OrdinalIgnoreCase);
    
    public static string IncludeFiles(string script)
    {
        var includeLines = script
            .Split('\n')
            .Select(x => x.Trim())
            .Where(x => x.StartsWith(Keywords.IncludeKeyword + " "))
            .ToArray();

        if (includeLines.Length > 0)
            Logger.Logger.Log("Include:\n" + string.Join("\n", includeLines));

        List<string> files = [];

        foreach (var includeLine in includeLines)
        {

            var line = includeLine.Replace("include ", "").Replace("\"", "");
            // Logger.Logger.Log("Path: " + line);

            string fullPath;

            fullPath = Path.Combine(Directory.GetCurrentDirectory(), line.StartsWith('@') ? line[1..] : line);

            fullPath = fullPath.Replace('\\', Path.DirectorySeparatorChar);
            fullPath = fullPath.Replace('/', Path.DirectorySeparatorChar);

            Logger.Logger.Log("Path: " + fullPath);

            if (!Directory.Exists(fullPath) && !File.Exists(fullPath + ".ql"))
                throw new FileNotFoundException($"Include file or directory not found: {fullPath}");

            if (!Directory.Exists(fullPath))
                fullPath += ".ql";
                
            script = script.Replace(includeLine + "\r\n", "")
                .Replace(includeLine + "\n", "")
                .Replace(includeLine, "");

            if (!Included.Add(fullPath))
            {
                Logger.Logger.Warn($"Skipped (already included): {fullPath}");
                continue;
            }
            
            var content = "";

            if (fullPath.EndsWith(".ql"))
                content = File.ReadAllText(fullPath);
            else
            {
                foreach (string file in Directory.GetFiles(fullPath))
                {
                    Logger.Logger.Log("Path: " + file);
                    if (!Included.Add(file))
                    {
                        Logger.Logger.Warn($"Skipped (already included): {fullPath}");
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
        
        return string.Join(Environment.NewLine, files) + Environment.NewLine + script;

    }
    
    public static (string outScript, Dictionary<string, string> dictionary) ExtractNumbers(string script)
    {
        Logger.Logger.Warn($"Extract Numbers");
        Dictionary<string, string> numberDictionary = [];
        
        var numberCounter = 0;

        // const string pattern = @"(?<![\p{L}_])\d+(?:\.\d+)?(?:[eE][+-]?\d+)?(?![\p{L}_])";
        const string pattern =
            @"(?<!___[A-Z]+_)\b\d+(?:\.\d+)?(?:[eE][+-]?\d+)?\b(?!___)";

        var result = Regex.Replace(script, pattern, match => 
        {
            var numberValue = match.Value;
            var key = $"___NUMBER_{numberCounter}___";
            
            numberDictionary[key] = numberValue;
            
            numberCounter++;
            
            Logger.Logger.Warn($"Key='{key}', Value='{numberValue}'");
            
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
            
            Logger.Logger.Log($"Key='{key}', Value='{value}'");
            
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
        
            Logger.Logger.Log($"Comment='{comment}'");
        
            return ""; // <- удаляем найденный комментарий
        });
        
        Logger.Logger.Succ("Comments cleared successfully");
        return result;
    }

}