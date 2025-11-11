using System.Text.Json;

namespace Qlang;

public class Settings()
{
    public bool Debug { get; set; }

    public void Save(string path = "")
    {
        File.WriteAllText(Path.Combine(path, "settings.json"), JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true }));
    }

    public static Settings? Load(string path = "")
    {
        if (!File.Exists(Path.Combine(path, "settings.json")))
            return null;
        
        string content = File.ReadAllText(Path.Combine(path, "settings.json"));
        
        Settings? settings = JsonSerializer.Deserialize<Settings>(content);

        return settings;
    }
}