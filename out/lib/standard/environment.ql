import "$lib/standard"

namespace std: {
    const environment = new Environment();
    private class Environment: {
        const currentDirectory = field(_): {
            fn get() => _native("std", "env", "getCurrentDirectory")
            fn set(<String> str): _native("std", "env", "setCurrentDirectory", str);
        };

        const newLine = field(_): {
            fn get() => _native("std", "env", "newLine")
        };

        const machineName = field(_): {
            fn get() => _native("std", "env", "machineName")
        };

        const processPath = field(_): {
            fn get() => _native("std", "env", "processPath")
        };

        const userName = field(_): {
            fn get() => _native("std", "env", "userName")
        };

        function exit(<Number> code = 0): _native("std", "env", "exit", code);
    }
}