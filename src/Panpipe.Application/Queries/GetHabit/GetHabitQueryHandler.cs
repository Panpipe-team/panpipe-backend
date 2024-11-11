using Ardalis.Result;
using MediatR;
using Panpipe.Application.Interfaces;
using Panpipe.Application.Specifications;
using Panpipe.Domain.Entities.HabitAggregate;
using Panpipe.Domain.Entities.HabitParamsAggregate;

namespace Panpipe.Application.Queries.GetHabit;

public class GetHabitQueryHandler: IRequestHandler<GetHabitQuery, Result<Tuple<Habit, HabitParams>>>
{
    private readonly IReadRepository<Habit> _habitRepository;
    private readonly IReadRepository<HabitParams> _habitParamsRepository;

    public GetHabitQueryHandler(
        IReadRepository<Habit> habitRepository, IReadRepository<HabitParams> habitParamRepository
    )
    {
        _habitRepository = habitRepository;
        _habitParamsRepository = habitParamRepository;
    }

    public async Task<Result<Tuple<Habit, HabitParams>>> Handle(
        GetHabitQuery request, CancellationToken cancellationToken
    )
    {
        var habitSpec = new HabitWithMarksSpecification(request.HabitId);

        var habit = await _habitRepository.FirstOrDefaultAsync(habitSpec, cancellationToken);

        if (habit is null)
        {
            return Result.NotFound($"Habit with id {request.HabitId} was not found");
        }

        var habitParamsSpec = new HabitParamsSpecification(habit.ParamsId);

        var habitParams = await _habitParamsRepository.FirstOrDefaultAsync(habitParamsSpec, cancellationToken);

        if (habitParams is null)
        {
            return Result.CriticalError(
                $"Habit params with id {habit.ParamsId} for habit with id {habit.Id} were not found"
            );
        }

        return Result.Success(new Tuple<Habit, HabitParams>(habit, habitParams));
    }
}
