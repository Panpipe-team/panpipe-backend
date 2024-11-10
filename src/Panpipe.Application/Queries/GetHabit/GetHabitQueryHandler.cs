using Ardalis.Result;
using MediatR;
using Panpipe.Application.Interfaces;
using Panpipe.Application.Specifications;
using Panpipe.Domain.Entities.HabitAggregate;
using Panpipe.Domain.Interfaces;

namespace Panpipe.Application.Queries.GetHabit;

public class GetHabitQueryHandler<T> : IRequestHandler<GetHabitQuery<T>, Result<AbstractHabit<T>>>
    where T : IHabitResultType
{
    private readonly IReadRepository<AbstractHabit<T>> _habitRepository;

    public GetHabitQueryHandler(IReadRepository<AbstractHabit<T>> habitRepository)
    {
        _habitRepository = habitRepository;
    }

    public async Task<Result<AbstractHabit<T>>> Handle(GetHabitQuery<T> request, CancellationToken cancellationToken)
    {
        var spec = new HabitWithMarksSpecification<T>(request.HabitId);

        var result = await _habitRepository.FirstOrDefaultAsync(spec);

        if (result is null)
        {
            return Result.NotFound($"Habit with id {request.HabitId} was not found");
        }

        return Result.Success(result);
    }
}
