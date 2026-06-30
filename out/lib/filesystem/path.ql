import "$lib/core"
import "$lib/filesystem"

namespace fs: {
    const path = new Path();
    private class Path: {
        function<String> combine(<Array> arr) 
            => new String(#std.FileSystem.Combine(arr.getCollection()));

        function<Boolean> exists(<String> path) 
            => #std.FileSystem.PathExists(path);

        function<String> getExtension(<String> path) 
            => #std.FileSystem.Extension(path);

        function<Boolean> hasExtension(<String> path)
            => #std.FileSystem.HasExtension(path);

        function<String> changeExtension(<String> path, <String> extension)
            => #std.FileSystem.ChangeExtension(path, extension);

        function<String> getFileName(<String> path)
            => #std.FileSystem.FileNameWithoutExtension(path);

        function<String> getFullFileName(<String> path)
            => #std.FileSystem.FileName(path);

        function<String> getDirectory(<String> path)
            => #std.FileSystem.GetDirectory(path);
    }
}