import "$lib/core";
import "$lib/filesystem";

namespace fs: {
    class File: {
        // Return type: bool
        // Returns true if file found
        function exists(const<String> path):
            return _native("std.filesystem.file_exists", path);

        // Override file content
        function setContent(const<String> path, const<String> content): {
            if exists(path) == false:
                File.create(path);

            _native("std.filesystem.set_content", path, _str(content));
        }

        // Append content to end file
        function appendContent(const<String> path, const<String> content): {
            if exists(path) == false:
                Throw.exception("file path '" + path + "' is not found");

            _native("std.filesystem.append_content", path, _str(content));
        }

        // Return type: string
        // Get file content
        function getContent(const<String> path): {
            if exists(path) == false:
                Throw.exception("file path '" + path + "' is not found");

            return String.new(_native("std.filesystem.get_content", path));
        }

        // Create file
        function create(const<String> path):
            _native("std.filesystem.file_create", path);

        // Remove file
        function remove(const<String> path): {
            if exists(path) == false:
                Throw.exception("file path '" + path + "' is not found");

            _native("std.filesystem.file_remove", path);
        }
    }
}