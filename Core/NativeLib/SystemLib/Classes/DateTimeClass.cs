namespace Core.NativeLib.SystemLib.Classes;

public class DateTimeClass : IQlangClass
{
    public string Name { get; init; } = "dateTime";

    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("wait", (Action<int>)Thread.Sleep),
            ("now_ticks", (Func<double>)(() => DateTime.Now.Ticks)),
            ("utc_now_ticks", (Func<double>)(() => DateTime.UtcNow.Ticks)),
            
            ("create_ticks", (Func<int, int, int, int, int, int, double>)((y, mon, d, h, m, s) => 
                new DateTime(y, mon, d, h, m, s).Ticks)),

            ("get_year", (Func<double, double>)(t => new DateTime((long)t).Year)),
            ("get_month", (Func<double, double>)(t => new DateTime((long)t).Month)),
            ("get_day", (Func<double, double>)(t => new DateTime((long)t).Day)),
            ("get_hour", (Func<double, double>)(t => new DateTime((long)t).Hour)),
            ("get_minute", (Func<double, double>)(t => new DateTime((long)t).Minute)),
            ("get_second", (Func<double, double>)(t => new DateTime((long)t).Second)),

            ("add_days", (Func<double, double, double>)((t, v) => new DateTime((long)t).AddDays(v).Ticks)),
            ("add_hours", (Func<double, double, double>)((t, v) => new DateTime((long)t).AddHours(v).Ticks)),
            
            ("format", (Func<double, string, string>)((t, s) => new DateTime((long)t).ToString(s))),
            
            ("diff_ticks", (Func<double, double, double>)((t1, t2) => (t1 - t2))), 
            ("from_seconds", (Func<double, double>)(s => TimeSpan.FromSeconds(s).Ticks))
        ];
    }
}