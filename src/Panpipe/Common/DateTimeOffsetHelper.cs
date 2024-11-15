namespace Panpipe.Common;

public static class DateTimeOffsetHelper
{
    public static DateTimeOffset GetTodaysMidnightUtc()
    {
        var nowUtc = DateTimeOffset.UtcNow;

        return nowUtc - nowUtc.TimeOfDay;
    }

    public static int GetDaysSpanToLastMonday(DayOfWeek dayOfWeek) => dayOfWeek switch
    {
        DayOfWeek.Monday => 0,
        DayOfWeek.Tuesday => -1,
        DayOfWeek.Wednesday => -2,
        DayOfWeek.Thursday => -3,
        DayOfWeek.Friday => -4,
        DayOfWeek.Saturday => -5,
        DayOfWeek.Sunday => -6,
        _ => throw new NotImplementedException(),
    };
}
