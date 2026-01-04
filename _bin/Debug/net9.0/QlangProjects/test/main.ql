include "$lib/base"

function main(const args): {
    Console.println("Hello, World!");
    Console.println("Args: " + args.toString());
    Console.println("Press Enter to exit...");
    Console.readln();
}