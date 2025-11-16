class Throw:
    function exception(let message):
        csharp("throw new System.Exception(\"" + message + "\");")