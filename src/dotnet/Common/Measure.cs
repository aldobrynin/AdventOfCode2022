using System.Diagnostics;

namespace Common;

public static class Measure
{
    public static TimeSpan Time(Action action)
    {
        var sw = Stopwatch.StartNew();
        action();
        return sw.Elapsed;
    }
}