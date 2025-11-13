using System.Text.RegularExpressions;

namespace Qlang.Compiler;

public static class PreCompile
{
    public static string IncludeFiles(string script)
    {
        string[] includeLines = script.Split('\n').Where(x => x.StartsWith("include ")).ToArray();

        Console.WriteLine("IncludeLines: \n" + string.Join("\n", includeLines));
        
        List<string> files = [];
        
        foreach (var includeLine in includeLines)
        {
            string line = includeLine.Replace("include ", "").Replace("\"", "");
            
            string @namespace = line[..line.IndexOf("/", StringComparison.Ordinal)];
            
            string fileName = line[(line.IndexOf("/", StringComparison.Ordinal)  + 1)..] + ".rs";
            
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), @namespace, fileName);
            
            
            if (!File.Exists(fullPath))
                throw new Exception($"The file '{fullPath}' was not found.");
         
            script = script.Replace($"include \"{@namespace}/{fileName}\"", "");
            string content = File.ReadAllText(Path.Combine(@namespace, fileName));

            string subScript = IncludeFiles(content);
            
            files.Add(subScript);
        }
        
        if (files.Count > 0)
            return string.Join(Environment.NewLine, files) + Environment.NewLine + script;

        return script;
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