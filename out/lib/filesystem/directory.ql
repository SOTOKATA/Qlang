import "$lib/core";
import "$lib/filesystem";

namespace fs: {
    class Directory: {

        // Return type: bool
        // Return true if directory exists
        function exists(const<String> path):
            return _native("std.filesystem.directory_exists", path);

        // Create if not exists directory
        function create(const<String> path): {
            if exists(path) == true:
                Throw.exception("Directory already created.");

            _native("std.filesystem.directory_create", path);
        }

        // Remove if exists directory (recursive)
        function remove(const<String> path): {
            if exists(path) == false:
                Throw.exception("Directory is not exists.");
            
            _native("std.filesystem.directory_remove", path, true);
        }
    }
}