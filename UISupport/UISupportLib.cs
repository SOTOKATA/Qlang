using Qlang.Core.Lang.Interpreter.Native;
using Qlang.Plugins;

public class UISupportLib : IQlangLib
{
    public string Name => "UISupport";
    public string Version => "0.0.1";

    public void Register(NativeFunctionRegistry registry)
    {
    }
}