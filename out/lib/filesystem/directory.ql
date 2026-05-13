import "$lib/core"
import "$lib/filesystem"

namespace fs: {
    const directory = new Directory();
    private class Directory: {

        // Return type: bool
        // Return true if directory exists
        function<Boolean> exists(<String> path) => #std.FileSystem.DirectoryExists(path);

        // Create if not exists directory
        function create(<String> path): {
            if exists(path):
                std::throw.message("Directory already created.");
            
            #std.FileSystem.DirectoryCreate(path);
        }

        // Remove if exists directory (recursive)
        function remove(<String> path): {
            if !exists(path):
                std::throw.message("Directory is not exists.");
            
            #std.FileSystem.DirectoryRemove(path, true);
        }

        function<Array> getFiles(<String> path, <String> extension = ""): {
            if !exists(path):
                std::throw.message("Undefined directory path.");

            return new Array(#std.FileSystem.GetFiles(path, extension));
        }

        function<Array> getDirectories(<String> path, <String> searchPattern = ""): {
            if !exists(path):
                std::throw.message("Undefined directory path.");

            return new Array(#std.FileSystem.GetDirectories(path, searchPattern));
        }
    }
}