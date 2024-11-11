using Ardalis.Result;
using MediatR;
using Panpipe.Application.Interfaces;
using Panpipe.Application.Specifications;
using Panpipe.Domain.Entities.HabitAggregate;
using Panpipe.Domain.Entities.HabitParamsAggregate;
using Panpipe.Domain.Services;

namespace Panpipe.Application.Commands.CreateUserHabitFromParams;

public class CreateUserHabitFromParamsCommandHandler : 
    IRequestHandler<CreateUserHabitFromParamsCommand, Result>
{
    private readonly IRepository<Habit> _habitRepository;
    private readonly IReadRepository<HabitParams> _habitParamsRepository;

    public CreateUserHabitFromParamsCommandHandler
    (
        IRepository<Habit> habitRepository, 
        IReadRepository<HabitParams> habitParamsRepository
    )
    {
        _habitRepository = habitRepository;
        _habitParamsRepository = habitParamsRepository;
    }

    public async Task<Result> Handle(CreateUserHabitFromParamsCommand request, CancellationToken cancellationToken)
    {
        var habitParamsSpecification = new HabitParamsSpecification(request.HabitParamsId);
        var habitParams = await _habitParamsRepository.FirstOrDefaultAsync(habitParamsSpecification, cancellationToken);

        if (habitParams is null)
        {
            return Result.NotFound($"Habit params with id {request.HabitParamsId} was not found");
        }

        var habit = UserHabit.CreateFromParams(Guid.NewGuid(), request.HabitParamsId, request.UserId);

        HabitService.AddEmptyMarksToNewlyCreatedHabit(habit, habitParams);

        await _habitRepository.AddAsync(habit, cancellationToken);

        await _habitRepository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}