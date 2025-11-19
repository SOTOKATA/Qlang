// Class to throw exceptions
class Throw:
    // Throw default exception
    function exception(let message):
        _csharp("throw new System.Exception(" + _str(message) + ");")

    function nonImplementException():
        exception("This function or object is not implement")