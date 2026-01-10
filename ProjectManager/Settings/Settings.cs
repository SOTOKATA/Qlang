using Core;
using ProjectManager.Project;

namespace ProjectManager.Settings;

public abstract class Settings(string path, Dictionary<string, (object? @object, Type type)>? dict)
{
    public Dictionary<string, (object? @object, Type type)>? GetDictionary() => Dictionary;
    public string GetPath() => path;

    public void Save()
    {
        if (Dictionary is null)
            throw new ProjectException("Settings dictionary is not set.");
        
        var serialized = Json.Serialize(Dictionary);
        
        if (!File.Exists(path))
            throw new FileNotFoundException($"Settings file '{path}' is not found.", path);
        
        File.WriteAllText(path, serialized);
    }

    public static Dictionary<string, (object? @object, Type type)> LoadDictionary<T>(string p) where T : Settings
    {
        if (!File.Exists(p))
            throw new FileNotFoundException($"Settings file '{p}' does not exist.");

        var dict = Json.Deserialize<Dictionary<string, (object? @object, Type type)>?>(File.ReadAllText(p));

        return dict ?? throw new ProjectException("Cannot load dictionary from file.");
    }
    
    public void Set(string param, object? value)
    {
        if (Dictionary is null)
            throw new ProjectException("Settings dictionary is not set.");
        
        if (!Dictionary.TryGetValue(param, out var dictValue))
            throw new ProjectException($"Key '{param}' did not exist.\nPath: {path}");

        if (dictValue.type != value?.GetType())
        {
            value = dictValue.@object switch
            {
                bool when bool.TryParse(value?.ToString(), out var boolValue) => boolValue,
                
                double when (value ?? "").ToString().TryParseNumber(out var num) => num,
                
                _ => throw new ProjectException($"Type of key '{param}' ({(dictValue.@object is null ? "Null" : dictValue.type)}) is not equal to type of '{value}' ({(value is null ? "Null" : value.GetType())})")
            };
        }
        
        Dictionary[param] = (value, value.GetType());
    }

    public object? Get(string param)
    {
        if (Dictionary is null)
            throw new ProjectException("Settings dictionary is not set.");
        
        if (!Dictionary.TryGetValue(param, out var value))
            throw new ProjectException($"Key '{param}' did not exist.\nPath: {path}");
        
        return value.@object;
    }
    
    public string GetString(string param)
    {
        if (Dictionary is null)
            throw new ProjectException("Settings dictionary is not set.");
        
        if (!Dictionary.TryGetValue(param, out var value))
            throw new ProjectException($"Key '{param}' did not exist.\nPath: {path}");
        
        if (value.type != typeof(string))
            throw new ProjectException($"Type of key '{param}' is not equal to type of '{(value.@object ?? "null")}'");
        
        return (string)value.@object!;
    }

    protected Dictionary<string, (object? @object, Type type)>? Dictionary = dict;
}