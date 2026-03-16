import "$lib/standard"

namespace std: {
    const environment = new Environment();
    private class Environment: {
        function getCurrentDirectory() => _native("std", "env", "current_directory");

        function getNewLine() => _native("std", "env", "new_line");
    
        function getMachineName() => _native("std", "env", "machine_name");

        function getProcessPath() => _native("std", "env", "process_path");

        function getUserName() => _native("std", "env", "user_name");

        function exit(const<Number> code = 0) => _native("std", "env", "exit", code);
    }
}