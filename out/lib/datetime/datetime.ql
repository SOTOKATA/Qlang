import "$lib/core"

namespace dt: {
    const datetime = new DateTime();
    class DateTime extends DataType: {
        
        function new(y, m, d, h = 0, min = 0, s = 0): 
            _value = _native("std", "dateTime", "create_ticks", y, m, d, h, min, s);

        function new(): _value = _native("std", "dateTime", "now_ticks");

        function now() => (new DateTime())._fromTicks(_native("std", "dateTime", "now_ticks"));

        function _fromTicks(ticks): {
            let dt = new DateTime(1, 1, 1);

            dt.setTicks(ticks);
            
            return dt;
        }

        function setTicks(ticks): _value = ticks;

        function year() => _native("std", "dateTime", "get_year", _value);
        function month() => _native("std", "dateTime", "get_month", _value);
        function day() => _native("std", "dateTime", "get_day", _value);

        function addDays(count) => _fromTicks(_native("std", "dateTime", "add_days", _value, count));

        function _operatorSubtraction(other): {
            const ticksDiff = _native("std", "dateTime", "diff_ticks", _value, other.getValue());
            
            return new TimeSpan(ticksDiff);
        }

        function toString(pattern = "yyyy-MM-dd HH:mm:ss") => _native("std", "dateTime", "format", _value, pattern);
    }
}