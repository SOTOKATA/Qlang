namespace Core.NativeLib.SystemLib.Classes;

public class FileSystemClass : IQlangClass
{
    public string Name { get; init; } = "FileSystem";
    
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("FileExists", (Func<string, bool>)(File.Exists)),
            ("SetContent", (Action<string, string>)File.WriteAllText),
            ("AppendContent", (Action<string, string>)File.AppendAllText),
            ("GetContent", (Func<string, string>)File.ReadAllText),
            ("FileCreate", (Action<string>)(path => File.Create(path).Close())),
            ("FileRemove", (Action<string>)File.Delete),

            ("Combine", (Func<List<object?>, string>)((arr) => Path.Combine(arr.Cast<string>().ToArray()))),
            ("Extension", (Func<string, string?>)Path.GetExtension),
            ("HasExtension", (Func<string, bool>)Path.HasExtension),
            ("ChangeExtension", (Func<string?, string?, string?>)Path.ChangeExtension),
            ("FileNameWithoutExtension", (Func<string, string?>)Path.GetFileNameWithoutExtension),
            ("FileName", (Func<string, string?>)Path.GetFileName),
            ("PathExists", (Func<string, bool>)Path.Exists),
            ("GetDir", (Func<string, string>)((path) => Path.GetDirectoryName(path) ?? "")),
            
            ("DirectoryExists", (Func<string, bool>)Directory.Exists),
            ("DirectoryCreate", (Action<string>)((path) => Directory.CreateDirectory(path))),
            ("DirectoryRemove", (Action<string, bool>)Directory.Delete),
            ("GetFiles", (Func<string, string, List<object?>>)((path, extension) => Directory.GetFiles(path, extension).Cast<object?>().ToList())),
            ("GetDirectories", (Func<string, string, List<object?>>)((path, searchPattern) => Directory.GetDirectories(path, searchPattern).Cast<object?>().ToList()))
        ];
    }
}