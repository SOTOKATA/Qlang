include "$lib/base"
include "$lib/filesystem"

class Path: {
    function combine(const<Array> arr):
        return String.new(_native("lib.filesystem.combine", arr.getCollection()));

    function exists(const<String> path):
        return _native("lib.filesystem.path_exists", path);

    function getExtension(const<String> path):
        return _native("lib.filesystem.extension", path);

    function hasExtension(const<String> path):
        return _native("lib.filesystem.has_extension", path);

    function changeExtension(const<String> path, const<String> extension):
        return _native("lib.filesystem.change_extension", path, extension);

    function getFileName(const<String> path):
        return _native("lib.filesystem.file_name_without_extension", path);

    function getFullFileName(const<String> path):
        return _native("lib.filesystem.file_name", path);
}