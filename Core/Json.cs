using System.Globalization;
using Newtonsoft.Json;

namespace Core;

public static class Json
{
    private static readonly JsonSerializerSettings JsonSettings = new()
    {
        TypeNameHandling = TypeNameHandling.Auto,
        DateFormatHandling =  DateFormatHandling.IsoDateFormat,
        DateTimeZoneHandling = DateTimeZoneHandling.Utc,
        TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
        DefaultValueHandling = DefaultValueHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore,
        Culture = CultureInfo.InvariantCulture
    };
    
    public static string Serialize(object obj, bool indented = false)
    {
        JsonSettings.Formatting = indented ? Formatting.Indented : Formatting.None;
        return JsonConvert.SerializeObject(obj, JsonSettings);
    }

    public static T? Deserialize<T>(string json, bool indented = false)
    {
        JsonSettings.Formatting = indented ? Formatting.Indented : Formatting.None;
        return JsonConvert.DeserializeObject<T>(json, JsonSettings);
    }
}