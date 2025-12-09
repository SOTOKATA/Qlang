include "$lib/base"

class Path: {
    function combine(let arr): {
        if (Array.isCollection(arr) == false) && (Array.isArray(arr) == false): {
            Throw.exception("Param must be Array or collection");
        }

        if Array.isArray(arr) == true: {
            arr = arr.getCollection();
        }

        return String.new(_native("path_combine", arr));
    }

    function exists(let path): {
        path = String.getPrimitive(path);

        return _native("path_exists", path);
    }

    function getExtension(let path): {
        path = String.getPrimitive(path);

        return _native("path_extension", path);
    }

    function hasExtension(let path): {
        path = String.getPrimitive(path);

        return _native("path_has_extension", path);
    }

    function changeExtension(let path, const extension): {
        path = String.getPrimitive(path);

        return _native("path_change_extension", path, extension);
    }

    function getFileName(let path): {
        path = String.getPrimitive(path);

        return _native("path_file_name_without_extension", path);
    }

    function getFullFileName(let path): {
        path = String.getPrimitive(path);
        
        return _native("path_file_name", path);
    }
}