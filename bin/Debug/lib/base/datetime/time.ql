class Time: {
    function wait(let millisec): {
        if Number.isNumber(millisec) == false: {
            Throw.parseException("argument 'milliseconds' is not a number");
        }

        millisec = Parser.asInt(millisec);

        _native("time_wait", millisec);
    }
}