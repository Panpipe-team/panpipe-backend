using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using Panpipe.Domain.Habit;
using Panpipe.Domain.HabitResult;
using Panpipe.Persistence;

namespace Panpipe.Controllers.Helpers;

public static class CommonOperations
{
    public async static Task<Result<float>> GetAnonymousStatistics(AppDbContext appDbContext, Guid habitId)
    {
        var currentHabit = await appDbContext.Habits
            .AsNoTracking()
            .Include(habit => habit.Marks)
                .ThenInclude(habitMark => habitMark.Result)
            .Where(habit => habit.Id == habitId)
            .FirstOrDefaultAsync();
        
        if (currentHabit is null)
        {
            return Result.NotFound($"Habit with id {habitId} was not found");
        }

        var habitParamsSet = await appDbContext.HabitParamsSets
            .AsNoTracking()
            .Include(habitParamsSet => habitParamsSet.Goal)
            .Where(habitParamsSet => habitParamsSet.Id == currentHabit.ParamsSetId)
            .FirstOrDefaultAsync();
        
        if (habitParamsSet is null)
        {
            return Result.CriticalError($"Habit params set with id {currentHabit.ParamsSetId} cannot be found");
        }

        var allHabitsExceptCurrentWithSameParams = await appDbContext.Habits
            .AsNoTracking()
            .Include(habit => habit.Marks)
                .ThenInclude(habitMark => habitMark.Result)
            .Where(habit => habit.ParamsSetId == habitParamsSet.Id && habit.Id != habitId)
            .ToListAsync();
        
        var goal = habitParamsSet.Goal;

        return CalculateStatistics(currentHabit, allHabitsExceptCurrentWithSameParams, goal);
    }

    public static Result<float> CalculateStatistics(
        Habit currentHabit, List<Habit> otherhabits, AbstractHabitResult goal
    )
    {
        var currentHabitStreak = currentHabit.GetStreak(goal);

        if (currentHabitStreak == 0)
        {
            return Result.Success(0f);
        }
        
        var otherHabitStreaks = otherhabits.Select(habit => habit.GetStreak(goal)).ToList();

        if (otherHabitStreaks.Count == 0)
        {
            const float betterThanEverybodyAsThereIsNobody = 1;

            return Result.Success(betterThanEverybodyAsThereIsNobody);
        }

        var countCurrentUserBetterStreak = otherHabitStreaks.Count(otherStreak => currentHabitStreak >= otherStreak);

        var currentUserBetterStreakProportion = 
            (float) (countCurrentUserBetterStreak + 1) / (otherHabitStreaks.Count + 1);

        return Result.Success(currentUserBetterStreakProportion);
    }
}
