include "$lib/base"

function main(): {
    p("Hello World!"));
}

function p(const a = String.new("")): {
    Console.println(a, true);

    return a == "Hello World!" ? "ext4" : "ntfs";
}