include "$lib/core"

function main(): {
    Console.println("Hello, World!");

    const runtime = Runtime.new();

    runtime.includeLib("$lib/core");
    runtime.includeLib("$lib/base");

    runtime.functionOverlayExecute("
Console.println(\"Hello from runtime!\");
");

    runtime.execute(@"
function main(): {
    Console.println("Hello from executed code!");
}
");
}