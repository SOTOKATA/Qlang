using System.Globalization;
using Newtonsoft.Json;

namespace Core;

public static class Json
{
    private static readonly JsonSerializerSettings JsonSettings = new()
    {
        TypeNameHandling = TypeNameHandling.Auto,
        TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
        DefaultValueHandling = DefaultValueHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore,
        Culture = CultureInfo.InvariantCulture
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