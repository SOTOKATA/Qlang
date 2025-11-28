include "$lib/base"

class Cmd: {
    function pt(): {
        Console.println("Hello World!");
    }

    function get(): {
        return String.new("H");
    }
}

function main(): {
    Console.println(Cmd.get().length());
}
