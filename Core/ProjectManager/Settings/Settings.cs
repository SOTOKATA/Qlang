using Qlang.Core.Lang;
using Qlang.Core.ProjectManager.Project;

namespace Qlang.Core.ProjectManager.Settings;

public abstract class Settings(string path, Dictionary<string, object?>? dict)
{
    public string GetPath() => path;

    public void Save()
    {
        string serialized = Json.Serialize(Dictionary);
        
        if (!File.Exists(path))
            throw new FileNotFoundException($"Settings file '{path}' is not found.", path);
        
        File.WriteAllText(path, serialized);
    }

    public static Dictionary<string, object?>? LoadDictionary(string p)
    {
        if (!File.Exists(p))
            throw new FileNotFoundException($"Settings file '{p}' does not exist.");
        
        return Json.Deserialize<Dictionary<string, object?>?>(File.ReadAllText(p));
    }
    
    public void Set(string param, object? value)
    {
        if (!Dictionary.TryGetValue(param, out var dictValue))
            throw new ProjectException($"Key '{param}' did not exist.\nPath: {path}");

        if (dictValue?.GetType() != value?.GetType())
        {
            value = dictValue switch
            {
                bool when bool.TryParse(value?.ToString(), out var boolValue) => boolValue,
                double when (value ?? "").ToString().TryParseNumber(out var num) => num,
                _ => throw new ProjectException($"Type of key '{param}' ({(dictValue is null ? "Null" : dictValue.GetType())}) is not equal to type of '{value}' ({(value is null ? "Null" : value.GetType())})")
            };
        }
        
        Dictionary[param] = value;
    }

    public object? Get(string param)
    {
        if (!Dictionary.TryGetValue(param, out var value))
            throw new ProjectException($"Key '{param}' did not exist.\nPath: {path}");
        
        return value;
    }
    
    public string GetString(string param)
    {
        if (!Dictionary.TryGetValue(param, out var value))
            throw new ProjectException($"Key '{param}' did not exist.\nPath: {path}");
        
        if (value?.GetType() != typeof(string))
            throw new ProjectException($"Type of key '{param}' is not equal to type of '{(value ?? "null")}'");
        
        return (string)value;
    }

    protected Dictionary<string, object?>? Dictionary = dict;
}