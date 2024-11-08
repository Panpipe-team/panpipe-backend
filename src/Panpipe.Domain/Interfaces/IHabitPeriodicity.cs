namespace Panpipe.Domain.Interfaces;

public interface IHabitPeriodicity {
    public List<DateTimeOffset> CalculateTimestampsOfEmptyMarksForNewlyCreatedHabit();

    public List<DateTimeOffset> CaluclateTimestampsOfEmptyMarksForExistingHabitFromLastMarkTimestamp
    (
        DateTimeOffset lastMarkTimestamp
    );
}
