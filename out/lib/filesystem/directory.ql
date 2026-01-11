include "$lib/core"
include "$lib/filesystem"

namespace filesystem: {
    class Directory: {

        // Return type: bool
        // Return true if directory exists
        function exists(const<String> path):
            return _native("lib.filesystem.directory_exists", path);

        // Create if not exists directory
        function create(const<String> path): {
            if exists(path) == true:
                Throw.exception("Directory already created.");

            _native("lib.filesystem.directory_create", path);
        }

        // Remove if exists directory (recursive)
        function remove(const<String> path): {
            if exists(path) == false:
                Throw.exception("Directory is not exists.");
            
            _native("lib.filesystem.directory_remove", path, true);
        }
    }
}