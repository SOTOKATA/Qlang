include "$lib/base"

class Directory: {

    // Return type: bool
    // Return true if directory exists
    function exists(let path): {
        return _native("directory_exists", path);
    }

    // Create if not exists directory
    function create(let path): {
        if exists(path) == true: {
            Throw.exception("Directory already created.");
        }

        _native("directory_create", path);
    }

    // Remove if exists directory (recursive)
    function remove(let path): {
        if exists(path) == false: {
            Throw.exception("Directory is not exists.");
        }
        
        _native("directory_remove", path, true);
    }
}