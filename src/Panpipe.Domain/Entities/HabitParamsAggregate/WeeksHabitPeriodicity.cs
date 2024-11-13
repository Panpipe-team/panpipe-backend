using Panpipe.Common.DateTime;

namespace Panpipe.Domain.Entities.HabitParamsAggregate;

public record WeeksHabitPeriodicity(Guid Id, int IntervalValue): AbstractHabitPeriodicity(Id, IntervalValue)
{
    private const int DaysInWeek = 7;

    public override HabitPeriodicityType Type => HabitPeriodicityType.Weeks;
    protected override int KeepMarksAheadAmount => 2;

    protected override DateTimeOffset GetCurrentMarkTimestamp()
    {
        var todayMidnightUtc = DateTimeOffsetHelper.GetTodaysMidnightUtc();

        var mondayMidnightUtc = todayMidnightUtc.AddDays
        (
            DateTimeOffsetHelper.GetDaysSpanToLastMonday(todayMidnightUtc.DayOfWeek)
        );

        return mondayMidnightUtc;
    }

    protected override DateTimeOffset GetMaxAheadMarkTimestampsFromNow()
    {
        var todayMidnightUtc = DateTimeOffsetHelper.GetTodaysMidnightUtc();

        return todayMidnightUtc.AddDays(IntervalValue * DaysInWeek * KeepMarksAheadAmount);
    }

    protected override DateTimeOffset GetNextMarkTimestamp(DateTimeOffset timestamp) 
        => timestamp.AddDays(IntervalValue * DaysInWeek);

    protected override void ValidateMarkTimestamp(DateTimeOffset timestamp)
    {
        EnsureDateTimeOffset.IsMondayMidnightUtc(timestamp);
    }
}
