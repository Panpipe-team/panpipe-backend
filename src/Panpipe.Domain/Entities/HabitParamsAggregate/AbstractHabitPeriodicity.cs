using Panpipe.Domain.Interfaces;

namespace Panpipe.Domain.Entities.HabitParamsAggregate;

public abstract class AbstractHabitPeriodicity: IHabitPeriodicity
{
    public AbstractHabitPeriodicity(int intervalValue) 
    {
        IntervalValue = intervalValue;
    }

    protected abstract int KeepMarksAheadAmount { get; }

    public int IntervalValue { get; }

    public List<DateTimeOffset> CalculateTimestampsOfEmptyMarksForNewlyCreatedHabit() 
    {
        var markTimestamps = new List<DateTimeOffset> { GetCurrentMarkTimestamp() };

        for (var i = 0; i < KeepMarksAheadAmount; i++) {
            markTimestamps.Add(GetNextMarkTimestamp(markTimestamps[i]));
        };
        
        return markTimestamps;
    }

    public List<DateTimeOffset> CaluclateTimestampsOfEmptyMarksForExistingHabitFromLastMarkTimestamp
    (
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
