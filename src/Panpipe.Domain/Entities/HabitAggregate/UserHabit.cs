using Panpipe.Domain.Events;

namespace Panpipe.Domain.Entities.HabitAggregate;

public static class UserHabit
{
    public static Habit CreateFromParams(Guid habitId, Guid paramsId, Guid userId)
    {
        var habit = new Habit(habitId, paramsId);

        habit.RaiseDomainEvent(new UserHabitCreatedEvent(habitId, userId));

        return habit;
    }
}
