using Panpipe.Domain.Entities.HabitAggregate;
using Panpipe.Domain.Entities.HabitParamsAggregate;
using Panpipe.Domain.Interfaces;

namespace Panpipe.Domain.Services;

public static class HabitService
{
    public static void AddEmptyMarksToNewlyCreatedHabit(Habit habit, HabitParams habitParams) 
    {
        ValidateHabitParamsRelateToHabit(habit, habitParams);

        var timestamps = habitParams.CalculateTimestampsOfEmptyMarksForNewlyCreatedHabit();

        foreach (var timestamp in timestamps)
        {
            habit.AddEmptyMark(Guid.NewGuid(), timestamp);
        }
    }

    public static void ChangeHabitResult(Habit habit, HabitParams habitParams, IHabitResult result, Guid markId)
    {
        ValidateHabitParamsRelateToHabit(habit, habitParams);

        if (!habitParams.HasSameResultType(result))
        {
            throw new InvalidOperationException
            (
                $"Trying to change result for habit with id {habit.Id}, but " +
                $"habit params result type: {habitParams.ResultType} does not correspond to new type: ${result.Type}"
            );
        }

        habit.ChangeResult(markId, result);
    }

    private static void ValidateHabitParamsRelateToHabit(Habit habit, HabitParams habitParams)
    {
        if (habit.ParamsId != habitParams.Id)
        {
            throw new InvalidOperationException
            (
                $"Trying to calculate timestamps of empty marks for habit with wrong params: " +
                $"habit has id {habit.Id} and params id {habit.ParamsId}" +
                $"while given params have id {habitParams.Id}"
            );
        }
    }
}