using Newtonsoft.Json;

namespace ProjectManager.Settings;

public class SettingsItem(object? value, Type type)
{
    // [JsonIgnore]
    public Type Type { get; set; } = type;
    public object? Value { get; set; } = value;
}