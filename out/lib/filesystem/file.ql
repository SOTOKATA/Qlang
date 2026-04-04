import "$lib/core"
import "$lib/filesystem"

namespace fs: {
    const file = new File();
    private class File: {
        // Return type: bool
        // Returns true if file found
        function<Boolean> exists(<String> path) => #std.FileSystem.FileExists(path);

        // Override file content
        function setContent(<String> path, <String> content): {
            if !exists(path):
                create(path);

            #std.FileSystem.SetContent(path, _str(content));
        }

        // Append content to end file
        function appendContent(<String> path, <String> content): {
            if !exists(path):
                std::throw.message("file path '" + path + "' is not found");

            #std.FileSystem.AppendContent(path, _str(content));
        }

        // Return type: string
        // Get file content
        function<String> getContent(<String> path): {
            if !exists(path):
                std::throw.message("file path '" + path + "' is not found");

            return new String(#std.FileSystem.GetContent(path));
        }

        // Create file
        function create(<String> path):
            #std.FileSystem.Create(path);

        // Remove file
        function remove(<String> path): {
            if !exists(path):
                std::throw.message("file path '" + path + "' is not found");

            #std.FileSystem.Remove(path);
        }
    }
}