using System.Text.RegularExpressions;

namespace Qlang.Compiler;

public static class PreCompile
{
    private static readonly HashSet<string> Included = new(StringComparer.OrdinalIgnoreCase);

    public static string IncludeFiles(string script)
    {
        var includeLines = script
            .Split('\n')
            .Select(x => x.Trim())
            .Where(x => x.StartsWith("include "))
            .ToArray();

        if (includeLines.Length > 0)
            Logger.Logger.Log("IncludeLines:\n" + string.Join("\n", includeLines));

        List<string> files = [];

        foreach (var includeLine in includeLines)
        {
            Logger.Logger.Log("ForEach: " + includeLine);

            string line = includeLine.Replace("include ", "").Replace("\"", "");
            Logger.Logger.Log("ForEach.Path: " + line);

            int slashIndex = line.IndexOf('/');
            if (slashIndex == -1)
                throw new Exception($"Invalid include path: '{line}' (must be namespace/filename)");

            string nameSpace = line[..slashIndex];
            string fileName = line[(slashIndex + 1)..];

            string fullPath = "";

            if (nameSpace.StartsWith('@'))
                fullPath = Path.Combine(Directory.GetCurrentDirectory(), nameSpace[1..], fileName) + ".ql";
            else
                fullPath = Path.Combine(Directory.GetCurrentDirectory(), nameSpace, fileName);

            Logger.Logger.Log("ForEach.FullPath: " + fullPath);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Include file not found: {fullPath}");

            script = script.Replace(includeLine + "\r\n", "")
                .Replace(includeLine + "\n", "")
                .Replace(includeLine, "");
            
            if (!Included.Add(fullPath))
            {
                Logger.Logger.Warn($"Skipped (already included): {fullPath}");
                continue;
            }

            string content = File.ReadAllText(fullPath);

            string subScript = IncludeFiles(content);

            files.Add(subScript);
        }

        if (files.Count <= 0) 
            return script;
        
        Logger.Logger.Succ("All includes processed successfully.");
        return string.Join(Environment.NewLine, files) + Environment.NewLine + script;

    }
    
    public static (string outScript, Dictionary<string, string> dictionary) ExtractStrings(string script)
    {
        Dictionary<string, string> stringDictionary = [];
        
        var stringCounter = 0;
        
        const string pattern = """
                               "(?:[^"\\]|\\.)*"
                               """;
        
        var result = Regex.Replace(script, pattern, match => 
        {
            var stringValue = match.Value;
            var key = $"___STRING_{stringCounter}___";
            
            stringDictionary[key] = stringValue.Substring(1, stringValue.Length - 2);
            
            stringCounter++;
            
            return key;
        });
        
        return (result, stringDictionary);
    }

    public static string ClearComments(string script)
    {
        const string pattern = @"//[^\r\n]*|/\*[\s\S]*?\*/";
        
        var result = Regex.Replace(script, pattern, "");
        
        return result;
    }
}