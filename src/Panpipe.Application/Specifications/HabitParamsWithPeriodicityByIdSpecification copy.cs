using Ardalis.Specification;
using Panpipe.Domain.Entities.HabitParamsAggregate;
using Panpipe.Domain.Interfaces;

namespace Panpipe.Application.Specifications;

public class HabitParamsWithPeriodicityByIdSpecification<T>: Specification<HabitParams<T>> where T: IHabitResultType
{
    public HabitParamsWithPeriodicityByIdSpecification(Guid id)
    {
        Query
            .Where(habitParams => habitParams.Id == id)
            .Include(habitParams => habitParams.Periodicity);
    }
}
