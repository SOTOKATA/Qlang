using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Qlang.Core.Lang.Interpreter.Native;
using Qlang.NativeLib;

public class UISupportLib : IQlangLib
{
    public string Name { get; }
    public string Version { get; }
    public string Author { get; }
    public string Class { get; }
    public string Namespace { get; }
    public List<(string name, Delegate body)> GetFunctions()
    {
        throw new NotImplementedException();
    }
}