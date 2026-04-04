import "$lib/standard"

namespace std: {
    const environment = new Environment();
    private class Environment: {
        const currentDirectory = field(_): {
            fn get() => #std.Env.GetCurrentDirectory()
            fn set(<String> str): #std.Env.SetCurrentDirectory(str);
        };

        const newLine = field(_): {
            fn get() => #std.Env.NewLine()
        };

        const machineName = field(_): {
            fn get() => #std.Env.MachineName()
        };

        const processPath = field(_): {
            fn get() => #std.Env.ProcessPath()
        };

        const userName = field(_): {
            fn get() => #std.Env.UserName()
        };

        function exit(<Number> code = 0): #std.Env.Exit(code);
    }
}