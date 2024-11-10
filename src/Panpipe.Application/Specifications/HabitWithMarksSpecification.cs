using Ardalis.Specification;
using Panpipe.Domain.Entities.HabitAggregate;
using Panpipe.Domain.Interfaces;

namespace Panpipe.Application.Specifications;

public class HabitWithMarksSpecification<T>: Specification<Habit<T>> where T: IHabitResultType
{
    public HabitWithMarksSpecification(Guid habitId)
    {
        Query
            .Where(habit => habit.Id == habitId)
            .Include(habit => habit.HabitMarks);
    }
}
