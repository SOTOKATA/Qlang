include "$lib/base"

function main(): {
    let o = String.new("Hello World!");
    const res = o + " Amigo!";
    Console.println(res.length());
}