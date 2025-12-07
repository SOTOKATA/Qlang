include "$lib/base"

class Path: {
    // function getExtension(let path): {
    //     return _csharp(usings + "Path.GetExtension(" + _str(path) + ")");
    // }
    function combine(let arr): {
        if (Array.isCollection(arr) == false) && (Array.isArray(arr) == false): {
            Throw.exception("Param must be Array or collection");
        }

        if Array.isArray(arr) == true: {
            arr = arr.getCollection();
        }

        return String.new(_native("path_combine", arr));
    }
    // function getDirSeparator(): {
    //     return _csharp(usings + "Path.DirectorySeparatorChar");
    // }       
    // function getAltDirSeparator(): {
    //     return _csharp(usings + "Path.AltDirectorySeparatorChar");
    // }
}