include "$lib/base"
include "$lib/filesystem"

class Path: {
    function combine(let arr): {
        if (Array.isCollection(arr) == false) && (Array.isArray(arr) == false): {
            Throw.exception("Param must be Array or collection");
        }

        if Array.isArray(arr) == true: {
            arr = arr.getCollection();
        }

        return String.new(_native("lib.filesystem.combine", arr));
    }

    function exists(let path): {
        path = String.getPrimitive(path);

        return _native("lib.filesystem.path_exists", path);
    }

    function getExtension(let path): {
        path = String.getPrimitive(path);

        return _native("lib.filesystem.extension", path);
    }

    function hasExtension(let path): {
        path = String.getPrimitive(path);

        return _native("lib.filesystem.has_extension", path);
    }

    function changeExtension(let path, const extension): {
        path = String.getPrimitive(path);

        return _native("lib.filesystem.change_extension", path, extension);
    }

    function getFileName(let path): {
        path = String.getPrimitive(path);

        return _native("lib.filesystem.file_name_without_extension", path);
    }

    function getFullFileName(let path): {
        path = String.getPrimitive(path);
        
        return _native("lib.filesystem.file_name", path);
    }
}