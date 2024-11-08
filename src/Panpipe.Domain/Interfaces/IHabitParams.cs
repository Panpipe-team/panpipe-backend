namespace Panpipe.Domain.Interfaces;

public interface IHabitParams
{
    public List<DateTimeOffset> CalculateTimestampsOfEmptyMarksForNewlyCreatedHabit();

    public List<DateTimeOffset> CaluclateTimestampsOfEmptyMarksForExistingHabitFromLastMarkTimestamp
    (
        DateTimeOffset lastMarkTimestamp
    );
}