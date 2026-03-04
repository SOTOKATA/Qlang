import "$lib/core"
import "$lib/filesystem"

namespace fs: {
    class Path: {
        function<String> combine(const<Array> arr):
            return new String(_native("std", "filesystem", "combine", arr.getCollection()));

        function<Boolean> exists(const<String> path):
            return _native("std", "filesystem", "path_exists", path);

        function<String> getExtension(const<String> path):
            return _native("std", "filesystem", "extension", path);

        function<Boolean> hasExtension(const<String> path):
            return _native("std", "filesystem", "has_extension", path);

        function<String> changeExtension(const<String> path, const<String> extension):
            return _native("std", "filesystem", "change_extension", path, extension);

        function<String> getFileName(const<String> path):
            return _native("std", "filesystem", "file_name_without_extension", path);

        function<String> getFullFileName(const<String> path):
            return _native("std", "filesystem", "file_name", path);

        function<String> getDirectory(const<String> path):
            return _native("std", "filesystem", "get_dir", path);
    }
}