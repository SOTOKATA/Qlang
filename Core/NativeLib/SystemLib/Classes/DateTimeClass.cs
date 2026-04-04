namespace Core.NativeLib.SystemLib.Classes;

public class DateTimeClass : IQlangClass
{
    public string Name { get; init; } = "DateTime";

    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("Wait", (Action<int>)Thread.Sleep),
            ("NowTicks", (Func<double>)(() => DateTime.Now.Ticks)),
            ("UtcNowTicks", (Func<double>)(() => DateTime.UtcNow.Ticks)),
            
            ("CreateTicks", (Func<int, int, int, int, int, int, double>)((y, mon, d, h, m, s) => 
                new DateTime(y, mon, d, h, m, s).Ticks)),

            ("GetYear", (Func<double, double>)(t => new DateTime((long)t).Year)),
            ("GetMonth", (Func<double, double>)(t => new DateTime((long)t).Month)),
            ("GetDay", (Func<double, double>)(t => new DateTime((long)t).Day)),
            ("GetHour", (Func<double, double>)(t => new DateTime((long)t).Hour)),
            ("GetMinute", (Func<double, double>)(t => new DateTime((long)t).Minute)),
            ("GetSecond", (Func<double, double>)(t => new DateTime((long)t).Second)),

            ("AddDays", (Func<double, double, double>)((t, v) => new DateTime((long)t).AddDays(v).Ticks)),
            ("AddHours", (Func<double, double, double>)((t, v) => new DateTime((long)t).AddHours(v).Ticks)),
            
            ("Format", (Func<double, string, string>)((t, s) => new DateTime((long)t).ToString(s))),
            
            ("DiffTicks", (Func<double, double, double>)((t1, t2) => (t1 - t2))), 
            ("FromSeconds", (Func<double, double>)(s => TimeSpan.FromSeconds(s).Ticks))
        ];
    }
}