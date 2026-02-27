using System.Text;
using System.Text.RegularExpressions;
using Core;
using Core.Exceptions;
using Core.Tables;

namespace Compiler;

public static class PreCompile
{
    private static readonly HashSet<string> Included = new(StringComparer.OrdinalIgnoreCase);

    public static (string script, List<QLIProgramLib> libs) IncludeFiles(string script, string fileName, List<QLIProgramLib> libs)
    {
        (script, libs) = IncludeNativeFoldersInFile(script, fileName, libs);
        
        var includeLines = script
            .Split('\n')
            .Select((line, index) => (Line: line.Trim(), LineNumber: index + 1))
            .Where(x => x.Line.StartsWith(Keywords.ImportKeyword + " ") && x.Line.EndsWith('"'))
            .ToArray();

        if (includeLines.Length < 1)
            return ($"#FILE \"{fileName}\"\n{script}", libs);

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
                        $"{Keywords.ImportKeyword} '{line}' Error: Process path is not found", index,
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
                throw new QlangCompileException($"{Keywords.ImportKeyword} file or directory not found: {fullPath}",
                    index, "Import", fileName);

            // Remove import line
            script = script.Replace(source, "");

            if (isDirectory)
            {
                foreach (var filePath in Directory.GetFiles(fullPath, "*.ql", SearchOption.TopDirectoryOnly))
                {
                    var path = Path.GetFullPath(filePath);
                   
                    if (!Included.Add(path))
                        continue;

                    var fileContent = File.ReadAllText(path);
                    (var processedContent, libs) = IncludeFiles(fileContent, path, libs);
                    includedContents.Add(processedContent);
                }
            }
            else
            {
                if (!fullPath.EndsWith(".ql"))
                    fullPath += ".ql";
                
                fullPath = Path.GetFullPath(fullPath);
                
                if (!Included.Add(fullPath))
                    continue;

                var fileContent = File.ReadAllText(fullPath);
                (var processedContent, libs) = IncludeFiles(fileContent, fullPath, libs);
                includedContents.Add(processedContent);
            }
        }

        StringBuilder result = new();

        foreach (var content in includedContents)
            result.AppendLine(content);

        result.AppendLine($"#FILE \"{fileName}\"");
        result.Append(script);

        return (result.ToString(), libs);
    }
    
    private static readonly HashSet<string> IncludedNative = new(StringComparer.OrdinalIgnoreCase);

    private static (string newScript, List<QLIProgramLib> dependencies) IncludeNativeFoldersInFile(string script, string fullPath, List<QLIProgramLib> libs)
    {
        var includeLines = script
            .Split('\n')
            .Select((line, index) => (Line: line.Trim(), LineNumber: index + 1))
            .Where(x => x.Line.StartsWith(Keywords.ImportNativeKeyword + " "))
            .ToArray();
        
        foreach (var (source, index) in includeLines)
        {
            var line = source.Replace($"{Keywords.ImportNativeKeyword} ", "").Replace("\"", "");

            if (line.StartsWith('$'))
            {
                var exePath = Path.GetDirectoryName(Environment.ProcessPath);

                if (exePath is null)
                    throw new QlangCompileException(
                        $"{Keywords.ImportNativeKeyword} '{line}' Error: Process path is not found", index,
                        "PreCompile", fullPath);

                fullPath = Path.Combine(exePath, line[1..]);
            }
            else
                fullPath = Path.Combine(Path.GetDirectoryName(fullPath) ?? "", line);

            fullPath = fullPath.Replace('\\', Path.DirectorySeparatorChar)
                .Replace('/', Path.DirectorySeparatorChar);

            var isDirectory = Directory.Exists(fullPath);

            if (File.Exists(fullPath))
                throw new QlangCompileException($"Unable to add an external file. ({fullPath})", index, "PreCompile", fullPath);

            if (!isDirectory)
                throw new QlangCompileException($"Directory not found: {fullPath}",
                    index, "PreCompile", fullPath);
            
            if (!IncludedNative.Add(fullPath))
                Console.WriteLine("warn: Skipped native include path (already included): " + fullPath);

            script = script.Replace(source, "");

            var dependentsPath = Path.Combine(fullPath, "deps");
            
            if (!Directory.Exists(dependentsPath))
                throw new QlangCompileException($"Directory '{dependentsPath}' is not found.",  index, "PreCompile", fullPath);

            var lib = new QLIProgramLib();

            var files = Directory.GetFiles(dependentsPath, "*.dll", SearchOption.AllDirectories).ToList();
            foreach (var folder in files.Where(folder => !lib.DependenciesFilePaths.Contains(folder)))
                lib.DependenciesFilePaths.Add(folder);

            files = Directory.GetFiles(fullPath, "*.dll", SearchOption.TopDirectoryOnly).ToList();
            
            foreach (var file in files.Where(file => !lib.MainFilePaths.Contains(file)))
                lib.MainFilePaths.Add(file);
            
            libs.Add(lib);
        }

        return (script, libs);
    }

    public static (string outScript, List<double> list) ExtractNumbers(string script)
    {
        List<double> numberList = [];

        const string pattern =
            @"(?<!___[A-Z]+_)\b\d+(?:\.\d+)?(?:[eE][+-]?\d+)?\b(?!___)";

        var result = Regex.Replace(script, pattern, match =>
        {
            var numberValue = match.Value;
            
            if (!numberValue.TryParseNumber(out var @double))
                throw new Exception($"Undefined type of value '{numberValue}'");

            if (numberList.IndexOf(@double) != -1)
                return $"___N{numberList.IndexOf(@double)}___";

            numberList.Add(@double);

            return $"___N{numberList.Count - 1}___";
        });

        return (result, numberList);
    }

    
    public static (string outScript, StringPoolTable list) ExtractStrings(string script, StringPoolTable list)
    {
        var stringList = list;
        
        var stringCounter = stringList.StringPool.Count;
        
        const string pattern = """
                               "(?:[^"\\]|\\.)*"
                               """;
        
        var result = Regex.Replace(script, pattern, match => 
        {
            var stringValue = match.Value;
            var key = stringCounter;

            var value = stringValue.Substring(1, stringValue.Length - 2);
            
            value = value.Replace("\"", "\"");

            if (stringList.StringPool.IndexOf(value) != -1)
            {
                var existedKey = stringList.Add(value);

                return $"___S{existedKey}___";
            }
            
            stringList.Add(value);
            
            stringCounter++;
            
            return $"___S{key}___";
        });
        
        return (result, stringList);
    }
    
    public static string ReturnFileStrings(string script, StringPoolTable stringList)
    {
        var result = Regex.Replace(script, @"^#FILE\s+(___S\d+___)", match =>
        {
            var key = match.Groups[1].Value; // #FILE ___S124___
            
            var smatch = Regex.Match(key, @"\d+");
            if (!smatch.Success)
                throw new Exception($"Undefined type of value '{key}'");
            
            var number = int.Parse(smatch.Value);
            
            return number < stringList.StringPool.Count  
                ? $"#FILE {stringList[number]}" 
                : match.Value;
        }, RegexOptions.Multiline);
    
        return result;
    }

    public static string ClearComments(string script)
    {
        const string pattern = @"//[^\r\n]*|/\*[\s\S]*?\*/";
        
        var result = Regex.Replace(script, pattern, _ => "");
        
        return result;
    }

    public static (string script, StringPoolTable stringList) AddStringInterpolation(string script, StringPoolTable stringList)
    {
        var result = Regex.Replace(script, @"\$___S\d+___", match =>
        {
            var founded = match.Value[1..];
            
            var smatch = Regex.Match(founded, @"\d+");
            if (!smatch.Success)
                throw new Exception($"Undefined type of value '{founded}'");
            
            var index = int.Parse(smatch.Value);
            
            var outString = stringList[index];

            stringList.StringPool[index] = ReplaceFormatting(outString);
            
            var result = $"String.new({founded}).format([{string.Join(", ", GetFormatting(outString))}])";

            
            // Console.WriteLine($"Replaced '{founded}' to '{result}'");
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