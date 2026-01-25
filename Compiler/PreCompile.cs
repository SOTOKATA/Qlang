using System.Text;
using System.Text.RegularExpressions;
using Core;
using Core.Debug;
using Core.Exceptions;
using Core.Native;

namespace Compiler;

public static class PreCompile
{
    private static readonly HashSet<string> Included = new(StringComparer.OrdinalIgnoreCase);

    public static string IncludeFiles(string script, string fileName)
    {
        var includeLines = script
            .Split('\n')
            .Select((line, index) => (Line: line.Trim(), LineNumber: index + 1))
            .Where(x => x.Line.StartsWith(Keywords.IncludeKeyword + " ") && x.Line.EndsWith('"'))
            .ToArray();

        if (includeLines.Length < 1)
            return script;

        List<string> includedContents = [];

        foreach (var (source, index) in includeLines)
        {
            // Removing unused things
            var line = source.Substring(
                source.IndexOf('"') + 1,
                source.LastIndexOf('"') - source.IndexOf('"') - 1
            );

            string fullPath;

            // Set path from compile directory
            if (line.StartsWith('$'))
            {
                var exePath = Path.GetDirectoryName(Environment.ProcessPath);

                if (exePath is null)
                    throw new QlangCompileException(
                        $"{Keywords.IncludeKeyword} '{line}' Error: Process path is not found", index,
                        "PreCompile/ImportFiles", fileName);

                fullPath = Path.Combine(exePath, line[1..]);
            }
            else
                fullPath = Path.Combine(Path.GetDirectoryName(fileName) ?? Directory.GetCurrentDirectory(), line);

            // Make path dependent to system
            fullPath = fullPath.Replace('\\', Path.DirectorySeparatorChar)
                .Replace('/', Path.DirectorySeparatorChar);

            var isDirectory = Directory.Exists(fullPath);
            var isFile = File.Exists(fullPath) || File.Exists(fullPath + ".ql");

            if (!isDirectory && !isFile)
                throw new QlangCompileException($"{Keywords.IncludeKeyword} file or directory not found: {fullPath}",
                    index, "Import", fileName);

            // Remove import line
            script = script.Replace(source, "");

            if (isDirectory)
            {
                foreach (var filePath in Directory.GetFiles(fullPath, "*.ql", SearchOption.TopDirectoryOnly))
                {
                    if (!Included.Add(filePath))
                        continue;

                    var fileContent = File.ReadAllText(filePath);
                    var processedContent = IncludeFiles(fileContent, filePath);
                    includedContents.Add(processedContent);
                }
            }
            else
            {
                if (!fullPath.EndsWith(".ql"))
                    fullPath += ".ql";

                if (!Included.Add(fullPath))
                    continue;

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

    public static (List<QLIProgramLib> dependencies, string newScript) IncludeNativeFolders(string script, string fileName, List<QLIProgramLib> dependencies)
    {
        Logger.Log($"Folder: " + fileName, "IncludeNativeFolders");

        var includeLines = script
            .Split('\n')
            .Select((line, index) => (Line: line.Trim(), LineNumber: index + 1))
            .Where(x => x.Line.StartsWith(Keywords.IncludeNativeKeyword + " "))
            .ToArray();

        if (includeLines.Length > 0)
            Logger.Log(string.Join(", ", includeLines), Keywords.IncludeNativeKeyword);

        var qliLib = new QLIProgramLib();
        
        foreach (var (source, index) in includeLines)
        {
            var line = source.Replace($"{Keywords.IncludeNativeKeyword} ", "").Replace("\"", "");

            string fullPath;

            if (line.StartsWith('$'))
            {
                var exePath = Path.GetDirectoryName(Environment.ProcessPath);

                if (exePath is null)
                    throw new QlangCompileException(
                        $"{Keywords.IncludeNativeKeyword} '{line}' Error: Process path is not found", index,
                        "PreCompile/IncludeNativeFolders", fileName);

                fullPath = Path.Combine(exePath, line[1..]);
            }
            else
                fullPath = Path.Combine(Directory.GetCurrentDirectory(), line);

            fullPath = fullPath.Replace('\\', Path.DirectorySeparatorChar)
                .Replace('/', Path.DirectorySeparatorChar);

            Logger.Log(fullPath, "Path");

            var isDirectory = Directory.Exists(fullPath);

            if (File.Exists(fullPath))
                throw new QlangCompileException($"Unable to add an external file. ({fullPath})", index, "PreCompile/IncludeNativeFolders", fileName);

            if (!isDirectory)
                throw new QlangCompileException($"Directory not found: {fullPath}",
                    index, "PreCompile/IncludeNativeFiles", fileName);

            script = script.Replace(source, "");

            var dependentsPath = Path.Combine(fullPath, "dependents");
            
            if (!Directory.Exists(dependentsPath))
                throw new QlangCompileException($"Directory '{dependentsPath}' is not found.",  index, "PreCompile/IncludeNativeFolders", fileName);

            var folders = Directory.GetFiles(dependentsPath, "*.dll", SearchOption.AllDirectories).ToList();

            foreach (var folder in folders.Where(folder => !qliLib.DependenciesFilePaths.Contains(folder)))
                qliLib.DependenciesFilePaths.Add(folder);

            folders.AddRange(Directory.GetFiles(fullPath, "*.dll", SearchOption.TopDirectoryOnly));
            
            foreach (var file in folders)
            {
                Logger.Log(file, "Path");

                if (!IncludedNative.Add(file))
                {
                    Logger.Log($"{file}", "Skipped (already included)");
                    continue;
                }
                
                qliLib.MainFilePaths.Add(file);
            }
        }
        
        dependencies.Add(qliLib);

        return (dependencies, script);
    }
    
    public static (string outScript, List<double> list) ExtractNumbers(string script)
    {
        Logger.Log($"Extract Numbers");

        List<double> numberList = [];

        const string pattern =
            @"(?<!___[A-Z]+_)\b\d+(?:\.\d+)?(?:[eE][+-]?\d+)?\b(?!___)";

        var result = Regex.Replace(script, pattern, match =>
        {
            var numberValue = match.Value;
            
            if (!numberValue.TryParseNumber(out var @double))
                throw new Exception($"Undefined type of value '{numberValue}'");

            if (numberList.IndexOf(@double) != -1)
                return $"___NUMBER_{numberList.IndexOf(@double)}___";

            numberList.Add(@double);

            return $"___NUMBER_{numberList.Count - 1}___";
        });

        Logger.Log("Numbers extracted successfully");
        return (result, numberList);
    }

    
    public static (string outScript, List<string> list) ExtractStrings(string script, List<string> list)
    {
        Logger.Log($"Extract Strings");
        List<string> stringList = list;
        
        var stringCounter = stringList.Count;
        
        const string pattern = """
                               "(?:[^"\\]|\\.)*"
                               """;
        
        var result = Regex.Replace(script, pattern, match => 
        {
            var stringValue = match.Value;
            var key = stringCounter;

            var value = stringValue.Substring(1, stringValue.Length - 2);
            
            value = value.Replace("\"", "\"\"");

            if (stringList.IndexOf(value) != -1)
            {
                var existedKey = stringList.IndexOf(value);

                Logger.Log($"key='{existedKey}', value='{value}'");
                return $"___STRING_{existedKey}___";
            }
            
            stringList.Add(value);
            
            stringCounter++;
            
            Logger.Log($"key='{key}', value='{value}'");
            
            return $"___STRING_{key}___";
        });
        
        Logger.Log("Strings extracted successfully");
        return (result, stringList);
    }
    
    public static string ReturnFileStrings(string script, List<string> stringList)
    {
        Logger.Log("Return File Strings");
    
        var result = Regex.Replace(script, @"^#FILE\s+(___STRING_\d+___)", match =>
        {
            var key = match.Groups[1].Value; // #FILE ___STRING_124___
            
            var smatch = Regex.Match(key, @"\d+");
            if (!smatch.Success)
                throw new Exception($"Undefined type of value '{key}'");
            
            var number = int.Parse(smatch.Value);
            
            return number < stringList.Count  
                ? $"#FILE {stringList[number]}" 
                : match.Value;
        }, RegexOptions.Multiline);
    
        Logger.Log("Strings file returned successfully");
        return result;
    }

    public static string ClearComments(string script)
    {
        Logger.Log("Clear Comments");
        const string pattern = @"//[^\r\n]*|/\*[\s\S]*?\*/";
        
        var result = Regex.Replace(script, pattern, match =>
        {
            var comment = match.Value;
        
            Logger.Log($"comment='{comment}'");

            return "";
        });
        
        Logger.Log("Comments cleared successfully");
        return result;
    }

    public static (string script, List<string> stringList) AddStringInterpolation(string script, List<string> stringList)
    {
        var result = Regex.Replace(script, @"\$___STRING_\d+___", match =>
        {
            var founded = match.Value[1..];
            
            var smatch = Regex.Match(founded, @"\d+");
            if (!smatch.Success)
                throw new Exception($"Undefined type of value '{founded}'");
            
            var index = int.Parse(smatch.Value);
            
            var outString = stringList[index];

            stringList[index] = ReplaceFormatting(outString);
            
            var result = $"String.new({founded}).format([{string.Join(", ", GetFormatting(outString))}])";

            
            Console.WriteLine($"Replaced '{founded}' to '{result}'");
            return result;
            
        });
        return (result, stringList);
    }
    
    private static string ReplaceFormatting(string input)
    {
        var counter = 0;
        return Regex.Replace(input, @"(?<!\{)\{([^\{\}]+)\}(?!\})", _ =>
        {
            var replacement = $"{{{counter}}}";
            counter++;
            // Console.WriteLine("Replacement: " + replacement);
            return replacement;
        });
    }

    private static List<string> GetFormatting(string input)
    {
        var result = new List<string>();

        var matches = Regex.Matches(input, @"(?<!\{)\{([^\{\}]+)\}(?!\})");

        result.AddRange(matches.Select(match => match.Groups[1].Value.Replace(@"\""", "")));

        return result;
    }
}