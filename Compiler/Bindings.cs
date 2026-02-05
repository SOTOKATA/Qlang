using System.Text.RegularExpressions;
using Core;

namespace Compiler;

public static class Bindings
{
    public static string UseBindings(string script)
    {
        // Console.WriteLine(script);
        const string pattern = @"^bind\s+`([^`]*)`\s+`([^`]*)`";

        var bindings = new List<Binding>();

        var result = Regex.Replace(script, pattern, match =>
        {
            var from = match.Groups[1].Value.Trim();
            var to   = match.Groups[2].Value.Trim();
            Console.WriteLine($"from: {from} to: {to}");

            if (Keywords.GetKeywords().Contains(from))
                throw new Exception($"Cannot rebind existed keyword '{from}'");

            bindings.Add(new Binding(from, to));
            return "";
        }, RegexOptions.Multiline);

        return bindings.Aggregate(result, ApplyBinding);
    }
    
    private static string ApplyBinding(string script, Binding binding)
    {
        var callPattern = $@"\b{Regex.Escape(binding.From)}\b\s*(.*)";
        Console.WriteLine($"callPattern: {callPattern}");
        return Regex.Replace(script, callPattern, match =>
        {
            var argsRaw = match.Groups[1].Value.Trim();

            var args = ParseArgs(argsRaw);

            return IncludeArgs(binding.To, args);
        });
    }
    
    private static Dictionary<int, string> ParseArgs(string argsRaw)
    {
        var result = new Dictionary<int, string>();
        if (string.IsNullOrWhiteSpace(argsRaw))
            return result;

        var parts = Regex.Matches(argsRaw, @"[^\s]+");

        for (var i = 0; i < parts.Count; i++)
            result[i + 1] = parts[i].Value;

        return result;
    }

    private static string IncludeArgs(string template, Dictionary<int, string> args)
    {
        return Regex.Replace(template, @"\$arg(\d+)", m =>
        {
            var index = int.Parse(m.Groups[1].Value);
            return args.TryGetValue(index, out var value)
                ? value
                : m.Value;
        });
    }
}