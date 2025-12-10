using Qlang.Core.Lang.Interpreter.Native;

namespace Qlang.Plugins;

public interface IQlangLib
{
    string Name { get; }
    string Version { get; }
    void Register(NativeFunctionRegistry registry);
}