using Core;
using ProjectManager.Project;

namespace ProjectManager.Settings;

public abstract class Settings(string path, Dictionary<string, SettingsItem>? dict)
{
    public Dictionary<string, SettingsItem>? GetDictionary() => Dictionary;
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

    public static Dictionary<string, SettingsItem> LoadDictionary<T>(string p) where T : Settings
    {
        if (!File.Exists(p))
            throw new FileNotFoundException($"Settings file '{p}' does not exist.");

        var dict = Json.Deserialize<Dictionary<string, SettingsItem>?>(File.ReadAllText(p));

        return dict ?? throw new ProjectException("Cannot load dictionary from file.");
    }
    
    public void Set(string param, object? value)
    {
        if (Dictionary is null)
            throw new ProjectException("Settings dictionary is not set.");
        
        if (!Dictionary.TryGetValue(param, out var dictValue))
            throw new ProjectException($"Key '{param}' did not exist.\nPath: {path}");

        if (dictValue.Type != value?.GetType())
        {
            value = dictValue.Value switch
            {
                bool when bool.TryParse(value?.ToString(), out var boolValue) => boolValue,
                
                double when (value ?? "").ToString().TryParseNumber(out var num) => num,
                
                _ => throw new ProjectException($"Type of key '{param}' ({(dictValue.Value is null ? "Null" : dictValue.Type)}) is not equal to type of '{value}' ({(value is null ? "Null" : value.GetType())})")
            };
        }
        
        Dictionary[param] = new SettingsItem(value, value!.GetType());
    }

    public object? Get(string param)
    {
        if (Dictionary is null)
            throw new ProjectException("Settings dictionary is not set.");
        
        if (!Dictionary.TryGetValue(param, out var value))
            throw new ProjectException($"Key '{param}' did not exist.\nPath: {path}");
        
        return value.Value;
    }
    
    public string GetString(string param)
    {
        if (Dictionary is null)
            throw new ProjectException("Settings dictionary is not set.");
        
        if (!Dictionary.TryGetValue(param, out var value))
            throw new ProjectException($"Key '{param}' did not exist.\nPath: {path}");
        
        if (value.Value?.GetType() != typeof(string))
            throw new ProjectException($"Type of key '{param}' is not equal to type of '{(value.Value ?? "null")}'");
        
        return (string)value.Value!;
    }
    
    public bool GetBool(string param)
    {
        if (Dictionary is null)
            throw new ProjectException("Settings dictionary is not set.");
        
        if (!Dictionary.TryGetValue(param, out var value))
            throw new ProjectException($"Key '{param}' did not exist.\nPath: {path}");
        
        if (value.Value?.GetType() != typeof(bool))
            throw new ProjectException($"Type of key '{param}' is not equal to type of '{(value.Value ?? "null")}'");
        
        return (bool)value.Value;
    }

    protected Dictionary<string, SettingsItem>? Dictionary = dict;
}