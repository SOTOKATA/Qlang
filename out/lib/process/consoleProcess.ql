include "$lib/standard"

class ConsoleProcess: {
    function run(const<String> command):
        return _native("std.console_command.run", command);
}