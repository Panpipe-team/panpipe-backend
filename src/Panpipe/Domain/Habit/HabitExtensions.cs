using Panpipe.Domain.HabitResult;

namespace Panpipe.Domain.Habit;

public static class HabitExtensions
{
    public static int GetStreak(this Habit habit, AbstractHabitResult goal)
    {
        var marksSorted = habit.Marks
            .Where(mark => mark.TimestampUtc <= DateTimeOffset.UtcNow)
            .OrderByDescending(mark => mark.TimestampUtc);

        int streak = 0;

        foreach (var mark in marksSorted)
        {
            var result = mark.Result;

            if (result is null)
            {
                break;
            }

            if (!goal.IsAchievedBy(result))
            {
                break;
            }

            streak++;
        }

        return streak;
    }
}
