using Panpipe.Domain.Interfaces;

namespace Panpipe.Domain.Services;

public static class HabitService
{
    public static void AddEmptyMarksToNewlyCreatedHabit(IHabit habit, IHabitParams habitParams) 
    {
        var timestamps = habitParams.CalculateTimestampsOfEmptyMarksForNewlyCreatedHabit();

        foreach (var timestamp in timestamps)
        {
            habit.AddEmptyMark(Guid.NewGuid(), timestamp);
        }
    }
}