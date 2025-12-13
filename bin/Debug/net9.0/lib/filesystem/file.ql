include "$lib/base"
include "$lib/filesystem"

class File: {
    // Return type: bool
    // Returns true if file found
    function exists(let path): {
        path = String.getPrimitive(path);

        return _native("lib.filesystem.file_exists", path);
    }

    // Override file content
    function setContent(let path, const content): {
        path = String.getPrimitive(path);

        if exists(path) == false: {
            File.create(path);
        }

        _native("lib.filesystem.set_content", path, _str(content));
    }

    // Append content to end file
    function appendContent(let path, const content): {
        path = String.getPrimitive(path);

        if exists(path) == false: {
            Throw.exception("file path '" + path + "' is not found");
        }

        _native("lib.filesystem.append_content", path, _str(content));
    }

    // Return type: string
    // Get file content
    function getContent(let path): {
        path = String.getPrimitive(path);

        if exists(path) == false: {
            Throw.exception("file path '" + path + "' is not found");
        }

        return String.new(_native("lib.filesystem.get_content", path));
    }

    // Create file
    function create(let path): {
        path = String.getPrimitive(path);

        _native("lib.filesystem.file_create", path);
    }

    // Remove file
    function remove(let path): {
        path = String.getPrimitive(path);
        
        if exists(path) == false: {
            Throw.exception("file path '" + path + "' is not found");
        }

        _native("lib.filesystem.file_remove", path);
    }
}