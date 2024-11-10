using Ardalis.Specification;
using Panpipe.Domain.Entities.HabitAggregate;
using Panpipe.Domain.Interfaces;

namespace Panpipe.Application.Specifications;

public class HabitsWithoutMarksByUserIdSpecification<T>: Specification<AbstractHabit<T>> where T: IHabitResultType
{
    public HabitsWithoutMarksByUserIdSpecification(Guid userId)
    {
        Query
            .Where(habit => habit.Id == habitId)
            .Include(habit => habit.HabitMarks);
    }
}
