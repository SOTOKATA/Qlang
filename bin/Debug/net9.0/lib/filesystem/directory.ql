include "$lib/base"
include "$lib/filesystem"

class Directory: {

    // Return type: bool
    // Return true if directory exists
    function exists(let path): {
        path = String.getPrimitive(path);

        return _native("lib.directory_exists", path);
    }

    // Create if not exists directory
    function create(let path): {
        path = String.getPrimitive(path);

        if exists(path) == true: {
            Throw.exception("Directory already created.");
        }

        _native("lib.directory_create", path);
    }

    // Remove if exists directory (recursive)
    function remove(let path): {
        path = String.getPrimitive(path);
        
        if exists(path) == false: {
            Throw.exception("Directory is not exists.");
        }
        
        _native("lib.directory_remove", path, true);
    }
}