include "$lib/base"

class File: {
    // Return type: bool
    // Returns true if file found
    function exists(let path): {
        return _native("file_exists", path);
    }

    // Override file content
    function setContent(let path, let content): {
        if exists(path) == false: {
            File.create(path);
        }

        _native("file_set_content", path, _str(content));
    }

    // Append content to end file
    function appendContent(let path, let content): {
        if exists(path) == false: {
            Throw.exception("file path '" + path + "' is not found");
        }

        _native("file_append_content", path, _str(content));
    }

    // Return type: string
    // Get file content
    function getContent(let path): {
        if exists(path) == false: {
            Throw.exception("file path '" + path + "' is not found");
        }

        return String.new(_native("file_get_content", path));
    }

    // Create file
    function create(let path): {
        _native("file_create", path);
    }

    // Remove file
    function remove(let path): {
        if exists(path) == false: {
            Throw.exception("file path '" + path + "' is not found");
        }

        _native("file_remove", path);
    }
}