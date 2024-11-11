using MediatR;
using Panpipe.Application.Interfaces;
using Panpipe.Domain.Entities.HabitAggregate;
using Panpipe.Application.Specifications;
using Ardalis.Result;
using Panpipe.Domain.Entities.HabitParamsAggregate;
using Panpipe.Domain.Services;
using Panpipe.Application.Helpers;

namespace Panpipe.Application.Commands.ChangeHabitResult;

public class ChangeHabitResultCommandHandler : IRequestHandler<ChangeHabitResultCommand, Result> 
{
    private readonly IRepository<Habit> _habitRepository;
    private readonly IReadRepository<HabitParams> _habitParamsRepository;


    public ChangeHabitResultCommandHandler
    (
        IRepository<Habit> habitRepository, 
        IReadRepository<HabitParams> habitParamsRepository
    )
    {
        _habitRepository = habitRepository;
        _habitParamsRepository = habitParamsRepository;
    }
    public async Task<Result> Handle(ChangeHabitResultCommand request, CancellationToken cancellationToken)
    {
        var specification = new HabitWithMarksSpecification(request.HabitId);

        var habit = await _habitRepository.FirstOrDefaultAsync(specification, cancellationToken);
        
        if (habit is null)
        {
            return Result.NotFound($"Habit with id {request.HabitId} was not found");
        }

        var habitParamsSpecification = new HabitParamsSpecification(habit.ParamsId);
        var habitParams = await _habitParamsRepository.FirstOrDefaultAsync(habitParamsSpecification, cancellationToken);

        if (habitParams is null)
        {
            return Result.NotFound($"For habit with id {habit.Id} params with id {habit.ParamsId} was not found");
        }

        var habitResultType = habitParams.ResultType;
        var isParseSuccessful = habitResultType.TryParse(request.Value, out var newValue);

        if (!isParseSuccessful)
        {
            return Result.Invalid(new ValidationError(
                $"Value string \"{request.Value}\" cannot be casted to habit result type {habitParams.ResultType}"
            ));
        }
        
        try {
            HabitService.ChangeHabitResult(habit, habitParams, newValue, request.MarkId);
        }
        catch (InvalidOperationException) {
            return Result.NotFound($"Mark with id {request.MarkId} was not found");
        }

        await _habitRepository.UpdateAsync(habit, cancellationToken);

        await _habitRepository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}