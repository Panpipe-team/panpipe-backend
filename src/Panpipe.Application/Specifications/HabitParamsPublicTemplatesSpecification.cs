using Ardalis.Specification;
using Panpipe.Domain.Entities.HabitParamsAggregate;

namespace Panpipe.Application.Specifications;

public class HabitParamsPublicTemplatesSpecification: Specification<HabitParams>
{
    public HabitParamsPublicTemplatesSpecification()
    {
        Query
            .Where(habitParams => habitParams.IsPublicTemplate)
            .Include(habitParams => habitParams.Goal);
    }
}
