import "$lib/standard"

namespace std: {
    class Environment: {
        const currentDirectory = _native("std", "env", "current_directory");

        const newLine = _native("std", "env", "new_line");

        const userName = _native("std", "env", "user_name");
    
        const machineName = _native("std", "env", "machine_name");

        const processPath = _native("std", "env", "process_path");
    }
}