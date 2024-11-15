using Panpipe.Common;

namespace Panpipe.Domain.HabitParamsSet;

public static class FrequencyExtensions
{
    public static List<DateTimeOffset> CalculateTimestampsOfEmptyMarksForNewlyCreatedHabit(
        this Frequency frequency
    )
        => GetCalculator(frequency)
            .CalculateTimestampsOfEmptyMarksForNewlyCreatedHabit();

    public static List<DateTimeOffset> CalculateTimestampsOfEmptyMarksForExistingHabitFromLastMarkTimestamp(
        this Frequency frequency, DateTimeOffset lastMarkTimestamp
    )
        => GetCalculator(frequency)
            .CalculateTimestampsOfEmptyMarksForExistingHabitFromLastMarkTimestamp(lastMarkTimestamp);

    private static AbstractFrequencyEmptyMarksTimestampsCalculator GetCalculator(Frequency frequency)
    {
        return frequency.IntervalType switch
        {
            IntervalType.Day => new DayFrequencyEmptyMarksTimestampsCalculator(frequency.IntervalValue),
            IntervalType.Week => new WeekFrequencyEmptyMarksTimestampsCalculator(frequency.IntervalValue),
            IntervalType.Month => new MonthFrequencyEmptyMarksTimestampsCalculator(frequency.IntervalValue),
            _ => throw new NotImplementedException(),
        };
    }
}

public abstract class AbstractFrequencyEmptyMarksTimestampsCalculator(int intervalValue)
{
    protected int IntervalValue { get; } = intervalValue;
    protected abstract int KeepMarksAheadAmount { get; }

    public List<DateTimeOffset> CalculateTimestampsOfEmptyMarksForNewlyCreatedHabit()
    {
        var markTimestamps = new List<DateTimeOffset> { GetCurrentMarkTimestamp() };

        for (var i = 0; i < KeepMarksAheadAmount; i++) {
            markTimestamps.Add(GetNextMarkTimestamp(markTimestamps[i]));
        };

        return markTimestamps;
    }

    public List<DateTimeOffset> CalculateTimestampsOfEmptyMarksForExistingHabitFromLastMarkTimestamp(
        DateTimeOffset lastMarkTimestamp
    )
    {
        ValidateMarkTimestamp(lastMarkTimestamp);

        var newMarkTimestamps = new List<DateTimeOffset> {};
        var maxTimestamp = GetMaxAheadMarkTimestampsFromNow();
        var nextTimestamp = GetNextMarkTimestamp(lastMarkTimestamp);

        while (nextTimestamp <= maxTimestamp) {
            newMarkTimestamps.Add(nextTimestamp);
            nextTimestamp = GetNextMarkTimestamp(nextTimestamp);
        }

        return newMarkTimestamps;
    }

    protected abstract DateTimeOffset GetCurrentMarkTimestamp();
    protected abstract DateTimeOffset GetNextMarkTimestamp(DateTimeOffset timestamp);
    protected abstract DateTimeOffset GetMaxAheadMarkTimestampsFromNow();
    protected abstract void ValidateMarkTimestamp(DateTimeOffset timestamp);
}

public class DayFrequencyEmptyMarksTimestampsCalculator(int intervalValue): 
    AbstractFrequencyEmptyMarksTimestampsCalculator(intervalValue)
{
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

public class WeekFrequencyEmptyMarksTimestampsCalculator(int intervalValue): 
    AbstractFrequencyEmptyMarksTimestampsCalculator(intervalValue)
{
    private const int DaysInWeek = 7;

    protected override int KeepMarksAheadAmount => 2;

    protected override DateTimeOffset GetCurrentMarkTimestamp()
    {
        var todayMidnightUtc = DateTimeOffsetHelper.GetTodaysMidnightUtc();

        var mondayMidnightUtc = todayMidnightUtc.AddDays(
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

public class MonthFrequencyEmptyMarksTimestampsCalculator(int intervalValue): 
    AbstractFrequencyEmptyMarksTimestampsCalculator(intervalValue)
{
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
        => timestamp.AddMonths(IntervalValue);

    protected override void ValidateMarkTimestamp(DateTimeOffset timestamp)
    {
        EnsureDateTimeOffset.IsFirstDayOfMonthMidngihtUtc(timestamp);
    }
}
