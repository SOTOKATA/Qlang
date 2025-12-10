include "$lib/special/basetest"

function main(): {
    BaseTest.runTest();
    
    Throw.exception();
}

async function atest(): {
    return String.new("Hello World!");
}