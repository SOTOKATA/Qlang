import "$lib/standard"

namespace dt:  {
    const time = new Time();
    private class Time: {
        function wait(let<Number> millisec): {
            millisec = std::parser.asInt(millisec);

            _native("std", "datetime", "wait", millisec);
        }
    }
}