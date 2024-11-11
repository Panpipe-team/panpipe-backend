using Ardalis.Specification;
using Panpipe.Domain.Entities.HabitAggregate;

namespace Panpipe.Application.Specifications;

public class HabitWithMarksSpecification: Specification<Habit>
{
    public HabitWithMarksSpecification(Guid habitId)
    {
        Query
            .Where(habit => habit.Id == habitId)
            .Include(habit => habit.HabitMarks)
                .ThenInclude(habitMark => habitMark.Result);
    }
}
