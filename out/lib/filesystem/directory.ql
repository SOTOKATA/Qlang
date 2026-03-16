import "$lib/core"
import "$lib/filesystem"

namespace fs: {
    const directory = new Directory();
    private class Directory: {

        // Return type: bool
        // Return true if directory exists
        function<Boolean> exists(const<String> path):
            return _native("std", "filesystem", "directory_exists", path);

        // Create if not exists directory
        function create(const<String> path): {
            if exists(path):
                std::throw.message("Directory already created.");

            _native("std", "filesystem", "directory_create", path);
        }

        // Remove if exists directory (recursive)
        function remove(const<String> path): {
            if !exists(path):
                std::throw.message("Directory is not exists.");
            
            _native("std", "filesystem", "directory_remove", path, true);
        }

        function<Array> getFiles(const<String> path, const<String> extension = ""): {
            if !exists(path):
                throw.message("Undefined directory path.");

            return _native("std", "filesystem", "get_files", path, extension);
        }

        function<Array> getDirectories(const<String> path, const<String> searchPattern = ""): {
            if !exists(path):
                throw.message("Undefined directory path.");

            return _native("std", "filesystem", "get_directories", path, searchPattern);
        }
    }
}