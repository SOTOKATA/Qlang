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
        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        Culture = CultureInfo.InvariantCulture
    };
    
    private static readonly JsonSerializerSettings JsonIndentedSettings = new()
    {
        Formatting = Formatting.Indented,
        TypeNameHandling = TypeNameHandling.Auto,
        DateFormatHandling =  DateFormatHandling.IsoDateFormat,
        DateTimeZoneHandling = DateTimeZoneHandling.Utc,
        TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
        DefaultValueHandling = DefaultValueHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore,
        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        Culture = CultureInfo.InvariantCulture
    };
    
    public static string Serialize(object obj, bool indented = false)
    {
        return JsonConvert.SerializeObject(obj, indented ? JsonIndentedSettings : JsonSettings);
    }

    public static T? Deserialize<T>(string json, bool indented = false)
    {
        return JsonConvert.DeserializeObject<T>(json, indented ? JsonIndentedSettings : JsonSettings);
    }
}