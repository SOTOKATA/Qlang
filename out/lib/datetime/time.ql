import "$lib/standard"

namespace dt:  {
    class Time: {
        function wait(let<Number> millisec): {
            millisec = std::Parser.asInt(millisec);

            _native("std", "datetime", "wait", millisec);
        }
    }
}