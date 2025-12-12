using Qlang.Core.Lang.Interpreter.Native;

namespace Qlang.NativeLib;

public interface IQlangLib
{
    string Name { get; }
    string Version { get; }
    void Register(NativeFunctionRegistry registry);
}