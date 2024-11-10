using Ardalis.Result;
using MediatR;
using Panpipe.Application.Interfaces;
using Panpipe.Application.Specifications;
using Panpipe.Domain.Entities.HabitAggregate;
using Panpipe.Domain.Interfaces;

namespace Panpipe.Application.Queries.GetHabits;

public class GetHabitsQueryHandler<T> : IRequestHandler<GetHabitsQuery<T>, Result<List<Habit<T>>>>
    where T : IHabitResultType
{
    private readonly IReadRepository<Habit<T>> _habitRepository;

    public GetHabitsQueryHandler(IReadRepository<Habit<T>> habitRepository)
    {
        _habitRepository = habitRepository;
    }
    
    public async Task<Result<List<Habit<T>>>> Handle(GetHabitsQuery<T> request, CancellationToken cancellationToken)
    {
        var spec = new HabitsWithoutMarksByUserIdSpecification<T>(request.UserId);

        var result = await _habitRepository.ListAsync(spec, cancellationToken);

        return Result.Success(result);
    }
}
