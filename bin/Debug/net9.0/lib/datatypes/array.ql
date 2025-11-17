include "@lib/datatypes/number"

class Array:
    private function _csharpArr(let qarray):
        return _csharp("var list = new List<object>(); list.AddRange(" +  +");")

    private private _to_csharpArr(let qarray):
        return _csharp("qarray")

    function empty():
        return _csharp("public static List<object> func(){ return []; } return func();")

    function at(let array, let index):
        if Number.isNumber(index) == false:
            Throw.exception("Index must be a number")

        return _csharp(_csharpArr(array) + " list[" + index + "]")

    function append(let array, let item):
        _csharp(_csharpArr(array) + "list.Add(item); return list;")

    