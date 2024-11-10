using Ardalis.Specification;
using Panpipe.Domain.Entities.HabitAggregate;
using Panpipe.Domain.Interfaces;

namespace Panpipe.Application.Specifications;

public class HabitsWithoutMarksByUserIdSpecification<T>: Specification<Habit<T>> where T: IHabitResultType
{
    public HabitsWithoutMarksByUserIdSpecification(Guid userId)
    {
        Query
            .Where(habit => HabitOwnerIsSpecificUser(habit, userId));
    }

    private static bool HabitOwnerIsSpecificUser(Habit<T> habit, Guid userId)
    {
        if (habit.HabitOwner is UserHabitOwner user && user.UserId == userId)
        {
            return true;
        }

        return false;
    }
}
