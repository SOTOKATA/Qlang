import "$lib/core"
import "$lib/filesystem"

namespace fs: {
    class Path: {
        function combine(const<Array> arr):
            return String.new(_native("std", "filesystem", "combine", arr.getCollection()));

        function exists(const<String> path):
            return _native("std", "filesystem", "path_exists", path);

        function getExtension(const<String> path):
            return _native("std", "filesystem", "extension", path);

        function hasExtension(const<String> path):
            return _native("std", "filesystem", "has_extension", path);

        function changeExtension(const<String> path, const<String> extension):
            return _native("std", "filesystem", "change_extension", path, extension);

        function getFileName(const<String> path):
            return _native("std", "filesystem", "file_name_without_extension", path);

        function getFullFileName(const<String> path):
            return _native("std", "filesystem", "file_name", path);
    }
}