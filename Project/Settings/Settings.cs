using Qlang.Dependencies;

namespace Qlang.Project.Settings;

public abstract class Settings(string path, Dictionary<string, object?>? dict)
{
    public string GetPath() => path;

    public void Save()
    {
        string serialized = Json.Serialize(Dictionary);
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
            throw new Exception($"Key '{param}' did not exist.\nPath: {path}");

        if (dictValue?.GetType() != value?.GetType())
        {
            if (value is string)
            {
                if (bool.TryParse(value.ToString(), out var boolValue))
                    value = boolValue;
                else if (value.ToString().TryParseNumber(out var num))
                    value = num;
            }
            else
            {
                throw new Exception($"Type of key '{param}' is not equal to type of '{value}'");
            }
        }
        
        Dictionary[param] = value;
    }

    public object? Get(string param)
    {
        if (!Dictionary.TryGetValue(param, out var value))
            throw new Exception($"Key '{param}' did not exist.\nPath: {path}");
        
        return value;
    }
    
    public string GetString(string param)
    {
        if (!Dictionary.TryGetValue(param, out var value))
            throw new Exception($"Key '{param}' did not exist.\nPath: {path}");
        
        if (value?.GetType() != typeof(string))
            throw new Exception($"Type of key '{param}' is not equal to type of '{(value ?? "null")}'");
        
        return (string)value;
    }

    protected Dictionary<string, object?>? Dictionary = dict;
}