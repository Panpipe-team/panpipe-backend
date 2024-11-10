using Ardalis.Result;
using MediatR;
using Panpipe.Application.Interfaces;
using Panpipe.Domain.Entities.HabitAggregate;
using Panpipe.Domain.Interfaces;

namespace Panpipe.Application.Queries.GetHabits;

public class GetHabitsQueryHandler<T> : IRequestHandler<GetHabitsQuery<T>, Result<List<AbstractHabit<T>>>>
    where T : IHabitResultType
{
    private readonly IReadRepository<AbstractHabit<T>> _habitRepository;

    public GetHabitsQueryHandler(IReadRepository<AbstractHabit<T>> habitRepository)
    {
        _habitRepository = habitRepository;
    }
    
    public Task<Result<List<AbstractHabit<T>>>> Handle(GetHabitsQuery<T> request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
