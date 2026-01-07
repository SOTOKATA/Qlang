include "$lib/core"
include "$lib/base"

namespace base: {

}

namespace system: {
   class Console: {
        function WriteLine(const message): {
              Console.println(message);
        }
   }

   class Console2 extends Object: {
        function WriteLine(const message): {
              Console.println(message);
        }
   }

   const var = "Hello, World!";

   function out():{
        Console.println("Output from system namespace");
   }
}

namespace system: {
    class OtherClass: {
        function DoSomething(): {
            Console.println("Doing something!");
        }
    }
}

function main(): {
    system::Console.WriteLine("Hello, World!");
    // system::out();
    // system::Console.WriteLine(system::Console2);
}
