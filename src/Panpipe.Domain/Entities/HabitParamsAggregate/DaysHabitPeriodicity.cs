using Panpipe.Common.DateTime;

namespace Panpipe.Domain.Entities.HabitParamsAggregate;

public class DaysHabitPeriodicity : AbstractHabitPeriodicity
{
    public DaysHabitPeriodicity(int intervalValue) : base(intervalValue) {}

    protected override int KeepMarksAheadAmount => 5;

    protected override DateTimeOffset GetCurrentMarkTimestamp() => DateTimeOffsetHelper.GetTodaysMidnightUtc();

    protected override DateTimeOffset GetMaxAheadMarkTimestampsFromNow()
    {
        var currentTimestamp = GetCurrentMarkTimestamp();

        return currentTimestamp.AddDays(IntervalValue * KeepMarksAheadAmount);
    }

    protected override DateTimeOffset GetNextMarkTimestamp(DateTimeOffset timestamp) 
        => timestamp.AddDays(IntervalValue);

    protected override void ValidateMarkTimestamp(DateTimeOffset timestamp)
    {
        EnsureDateTimeOffset.IsMidnightUtc(timestamp);
    }
}
