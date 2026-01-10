include "$lib/standard"

class ConsoleProcess: {
    function run(const<String> command):
        return _native("lib.console_command.run", command);
}