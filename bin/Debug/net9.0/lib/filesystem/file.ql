class File: {
    function exists(let message): {
        return _csharp(usings + "File.Exists(" + _str(message) + ")");
    }

    function setContent(let path, let message): {
        Throw.nonImplementException();
    }
}