include "$lib/base"

class Func: {
    private const funcs;

    function new(const func): {
        funcs = func;
    }

    function invoke(const args = []): {
        return _native("function_invoke", funcs, args);
    }
}