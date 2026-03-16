import "$lib/core"
import "$lib/filesystem"

namespace fs: {
    const path = new Path();
    private class Path: {
        function<String> combine(const<Array> arr) 
            => new String(_native("std", "filesystem", "combine", arr.getCollection()));

        function<Boolean> exists(const<String> path) 
            =>  _native("std", "filesystem", "path_exists", path);

        function<String> getExtension(const<String> path) 
            => _native("std", "filesystem", "extension", path);

        function<Boolean> hasExtension(const<String> path)
            => _native("std", "filesystem", "has_extension", path);

        function<String> changeExtension(const<String> path, const<String> extension)
            => _native("std", "filesystem", "change_extension", path, extension);

        function<String> getFileName(const<String> path)
            => _native("std", "filesystem", "file_name_without_extension", path);

        function<String> getFullFileName(const<String> path)
            => _native("std", "filesystem", "file_name", path);

        function<String> getDirectory(const<String> path)
            => _native("std", "filesystem", "get_dir", path);
    }
}