using Ardalis.Specification;
using Panpipe.Domain.Entities.HabitAggregate;

namespace Panpipe.Application.Specifications;

public class HabitByIdsSpecification: Specification<Habit>
{
    public HabitByIdsSpecification(List<Guid> habitIds)
    {
        Query
            .Where(habit => habitIds.Contains(habit.Id));
    }
}
