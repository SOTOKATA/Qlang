import "$lib/core"

namespace dt: {
    const datetime = new DateTime();
    class DateTime extends DataType: {
        
        function new(y, m, d, h = 0, min = 0, s = 0): 
            _value = #std.DateTime.CreateTicks(y, m, d, h, min, s);

        function new(): _value = #std.DateTime.NowTicks();

        function now() => new DateTime()._fromTicks(#std.DateTime.NowTicks());

        function _fromTicks(ticks): {
            let dt = new DateTime(1, 1, 1);

            dt.setTicks(ticks);
            
            return dt;
        }

        function setTicks(ticks): _value = ticks;

        function year() => #std.DateTime.GetYear(_value);
        function month() => #std.DateTime.GetMonth(_value);
        function day() => #std.DateTime.GetDay(_value);

        function addDays(count) => _fromTicks(#std.DateTime.AddDays(_value), count);

        function _operatorSubtraction(other): {
            const ticksDiff = #std.DateTime.DiffTicks(_value, other.getValue());
            
            return new TimeSpan(ticksDiff);
        }

        function toString(pattern = "yyyy-MM-dd HH:mm:ss") => #std.DateTime.Format(_value, pattern);
    }
}