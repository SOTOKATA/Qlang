include "$lib/core"
include "$lib/standard"

namespace std:  {
    class Time: {
        function wait(let<Number> millisec): {
            if Number.isNumber(millisec) == false: {
                Throw.parseException("argument 'milliseconds' is not a number");
            }

            millisec = Parser.asInt(millisec);

            _native("lib.datetime.wait", millisec);
        }
    }
}