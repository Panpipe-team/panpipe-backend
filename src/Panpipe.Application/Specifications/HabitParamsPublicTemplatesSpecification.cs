using Ardalis.Specification;
using Panpipe.Domain.Entities.HabitParamsAggregate;
using Panpipe.Domain.Interfaces;

namespace Panpipe.Application.Specifications;

public class HabitParamsPublicTemplatesSpecification<T>: Specification<HabitParams<T>> where T: IHabitResultType
{
    public HabitParamsPublicTemplatesSpecification()
    {
        Query
            .Where(habitParams => habitParams.IsPublicTemplate)
            .Include(habitParams => habitParams.Periodicity);
    }
}
