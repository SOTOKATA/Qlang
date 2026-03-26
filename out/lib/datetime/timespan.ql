import "$lib/core"

namespace dt: {
    const timespan = new TimeSpan(0);
    class TimeSpan extends DataType: {
        
        function new(ticks):
            _value = ticks;

        function fromSeconds(s) => new TimeSpan(_native("std", "dateTime", "from_seconds", s));

        function totalSeconds() => _value / 10000000;
        function totalMinutes() => totalSeconds() / 60;
        function totalHours() => totalMinutes() / 60;
        function totalDays() => totalHours() / 24;

        function toString() => `{totalSeconds()} seconds`;
    }
}