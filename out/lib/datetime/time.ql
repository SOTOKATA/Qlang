import "$lib/core";
import "$lib/standard";

namespace dt:  {
    class Time: {
        function wait(let<Number> millisec): {
            if Number.isNumber(millisec) == false: {
                Throw.parseException("argument 'milliseconds' is not a number");
            }

            millisec = Parser.asInt(millisec);

            _native("std.datetime.wait", millisec);
        }
    }
}