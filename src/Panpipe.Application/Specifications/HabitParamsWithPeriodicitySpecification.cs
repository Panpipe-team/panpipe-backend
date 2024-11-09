using Ardalis.Specification;
using Panpipe.Domain.Entities.HabitParamsAggregate;
using Panpipe.Domain.Interfaces;

namespace Panpipe.Application.Specifications;

public class HabitParamsWithPeriodicitySpecification<T>: Specification<HabitParams<T>> where T: IHabitResultType
{
    public HabitParamsWithPeriodicitySpecification(Guid id)
    {
        Query
            .Where(habitParams => habitParams.Id == id)
            .Include(habitParams => habitParams.Periodicity);
    }
}