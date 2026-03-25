import "$lib/core"
import "$lib/filesystem"

namespace fs: {
    const path = new Path();
    private class Path: {
        function<String> combine(<Array> arr) 
            => new String(_native("std", "filesystem", "combine", arr.getCollection()));

        function<Boolean> exists(<String> path) 
            =>  _native("std", "filesystem", "path_exists", path);

        function<String> getExtension(<String> path) 
            => _native("std", "filesystem", "extension", path);

        function<Boolean> hasExtension(<String> path)
            => _native("std", "filesystem", "has_extension", path);

        function<String> changeExtension(<String> path, <String> extension)
            => _native("std", "filesystem", "change_extension", path, extension);

        function<String> getFileName(<String> path)
            => _native("std", "filesystem", "file_name_without_extension", path);

        function<String> getFullFileName(<String> path)
            => _native("std", "filesystem", "file_name", path);

        function<String> getDirectory(<String> path)
            => _native("std", "filesystem", "get_dir", path);
    }
}