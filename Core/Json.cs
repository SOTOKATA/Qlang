using Newtonsoft.Json;

namespace Qlang.Core;

public static class Json
{
    private static readonly JsonSerializerSettings JsonSettings = new()
    {
        Formatting = Formatting.Indented,
        TypeNameHandling = TypeNameHandling.Auto,
        TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
    };
    
    public static string Serialize(object obj)
    {
        return JsonConvert.SerializeObject(obj, JsonSettings);
    }

    public static T? Deserialize<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json, JsonSettings);
    }
}