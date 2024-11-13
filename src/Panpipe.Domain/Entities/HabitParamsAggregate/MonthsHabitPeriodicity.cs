using Panpipe.Common.DateTime;

namespace Panpipe.Domain.Entities.HabitParamsAggregate;

public record MonthsHabitPeriodicity(Guid Id, int IntervalValue) : AbstractHabitPeriodicity(Id, IntervalValue)
{
    public override HabitPeriodicityType Type => HabitPeriodicityType.Months;
    protected override int KeepMarksAheadAmount => 1;
    
    protected override DateTimeOffset GetCurrentMarkTimestamp() 
    {
        var nowUtc = DateTimeOffset.UtcNow;

        return new DateTimeOffset(nowUtc.Year, nowUtc.Month, 1, 0, 0, 0, TimeSpan.Zero);
    }

    protected override DateTimeOffset GetMaxAheadMarkTimestampsFromNow()
    {
        var currentTimestamp = GetCurrentMarkTimestamp();

        return currentTimestamp.AddMonths(IntervalValue * KeepMarksAheadAmount);
    }

    protected override DateTimeOffset GetNextMarkTimestamp(DateTimeOffset timestamp)
    {
        return timestamp.AddMonths(IntervalValue);
    }

    protected override void ValidateMarkTimestamp(DateTimeOffset timestamp)
    {
        EnsureDateTimeOffset.IsFirstDayOfMonthMidngihtUtc(timestamp);
    }
}
