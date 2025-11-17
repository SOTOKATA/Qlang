class Path:
    private let usings = "using System.IO; "
    function getExtension(let path):
        return _csharp(usings + "Path.GetExtension(" + _str(path) + ")")

    function combine(let first, let second):
        return _csharp(usings + "Path.Combine(" + _str(first) + ", " + _str(second) + ")")

    function getDirSeparator():
        return _csharp(usings + "Path.DirectorySeparatorChar")
        
    function getAltDirSeparator():
        return _csharp(usings + "Path.AltDirectorySeparatorChar")