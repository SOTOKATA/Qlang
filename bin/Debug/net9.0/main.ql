// include "$lib/special/snake"
include "$lib/base"

function main(): {
    Console.println(Ex.var);

    Ex.var = 2;

    Console.println(Ex.var);
}

class Ex: {
    let var = 1;
}