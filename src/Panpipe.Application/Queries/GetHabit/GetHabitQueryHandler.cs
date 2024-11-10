using Ardalis.Result;
using MediatR;
using Panpipe.Application.Interfaces;
using Panpipe.Application.Specifications;
using Panpipe.Domain.Entities.HabitAggregate;
using Panpipe.Domain.Interfaces;

namespace Panpipe.Application.Queries.GetHabit;

public class GetHabitQueryHandler<T> : IRequestHandler<GetHabitQuery<T>, Result<Habit<T>>>
    where T : IHabitResultType
{
    private readonly IReadRepository<Habit<T>> _habitRepository;

    public GetHabitQueryHandler(IReadRepository<Habit<T>> habitRepository)
    {
        _habitRepository = habitRepository;
    }

    public async Task<Result<Habit<T>>> Handle(GetHabitQuery<T> request, CancellationToken cancellationToken)
    {
        var spec = new HabitWithMarksSpecification<T>(request.HabitId);

        var result = await _habitRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (result is null)
        {
            return Result.NotFound($"Habit with id {request.HabitId} was not found");
        }

        return Result.Success(result);
    }
}
