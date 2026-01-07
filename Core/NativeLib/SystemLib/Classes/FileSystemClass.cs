namespace Core.NativeLib.SystemLib.Classes;

public class FileSystemClass : IQlangClass
{
    public string Name { get; init; } = "filesystem";
    
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("file_exists", (Func<string, bool>)File.Exists),
            ("set_content", (Action<string, string>)File.WriteAllText),
            ("append_content", (Action<string, string>)File.AppendAllText),
            ("get_content", (Func<string, string>)File.ReadAllText),
            ("file_create", (Action<string>)(path => File.Create(path).Close())),
            ("file_remove", (Action<string>)File.Delete),

            ("combine", (Func<List<object>, string>)((arr) => Path.Combine(arr.Cast<string>().ToArray()))),
            ("extension", (Func<string, string?>)Path.GetExtension),
            ("has_extension", (Func<string, bool>)Path.HasExtension),
            ("change_extension", (Func<string?, string?, string?>)Path.ChangeExtension),
            ("file_name_without_extension", (Func<string, string?>)Path.GetFileNameWithoutExtension),
            ("file_name", (Func<string, string?>)Path.GetFileName),
            ("path_exists", (Func<string, bool>)Path.Exists),
            
            ("directory_exists", (Func<string, bool>)Directory.Exists),
            ("directory_create", (Action<string>)((path) => Directory.CreateDirectory(path))),
            ("directory_remove", (Action<string, bool>)Directory.Delete),
        ];
    }
}