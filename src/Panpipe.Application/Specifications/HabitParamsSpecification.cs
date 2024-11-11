using Ardalis.Specification;
using Panpipe.Domain.Entities.HabitParamsAggregate;

namespace Panpipe.Application.Specifications;

public class HabitParamsSpecification: Specification<HabitParams>
{
    public HabitParamsSpecification(Guid id)
    {
        Query
            .Where(habitParams => habitParams.Id == id)
            .Include(habitParams => habitParams.Goal);
    }
}
