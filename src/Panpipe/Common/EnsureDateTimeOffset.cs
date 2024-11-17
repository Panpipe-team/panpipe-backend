namespace Panpipe.Common;

public static class EnsureDateTimeOffset
{
    public static void IsInUtc(DateTimeOffset dateTimeOffset) {
        if (dateTimeOffset.Offset != TimeSpan.Zero)
            throw new ArgumentException($"DateTimeOffset {dateTimeOffset} is not in UTC");
    }

    public static void IsMidnight(DateTimeOffset dateTimeOffset) {
        if (dateTimeOffset.TimeOfDay != TimeSpan.Zero)
            throw new ArgumentException($"DateTimeOffset {dateTimeOffset} is not midnight");
    }

    public static void IsMonday(DateTimeOffset dateTimeOffset) {
        if (dateTimeOffset.DayOfWeek != DayOfWeek.Monday)
            throw new ArgumentException($"DateTimeOffset {dateTimeOffset} is not Monday");
    }

    public static void IsFirstDayOfMonth(DateTimeOffset dateTimeOffset) {
        if (dateTimeOffset.Day != 1)
            throw new ArgumentException($"DateTimeOffset {dateTimeOffset} is not first day of month");
    }

    public static void IsMidnightUtc(DateTimeOffset dateTimeOffset) {
        IsInUtc(dateTimeOffset);
        IsMidnight(dateTimeOffset);
    }

    public static void IsMondayMidnightUtc(DateTimeOffset dateTimeOffset) {
        IsMidnightUtc(dateTimeOffset);
        IsMonday(dateTimeOffset);
    }

    public static void IsFirstDayOfMonthMidngihtUtc(DateTimeOffset dateTimeOffset) {
        IsMidnightUtc(dateTimeOffset);
        IsFirstDayOfMonth(dateTimeOffset);
    }
}
