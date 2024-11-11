using Ardalis.Specification;
using Panpipe.Domain.Entities.HabitParamsAggregate;

namespace Panpipe.Application.Specifications;

public class HabitParamsByIdsSpecification: Specification<HabitParams>
{
    public HabitParamsByIdsSpecification(List<Guid> ids)
    {
        Query
            .Where(habitParams => ids.Contains(habitParams.Id));
    }
}
