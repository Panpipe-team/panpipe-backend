using Ardalis.Result;
using MediatR;
using Panpipe.Application.Interfaces;
using Panpipe.Application.Specifications;
using Panpipe.Domain.Entities.HabitAggregate;
using Panpipe.Domain.Entities.HabitParamsAggregate;
using Panpipe.Domain.Interfaces;
using Panpipe.Domain.Services;

namespace Panpipe.Application.Commands.CreateUserHabitFromParams;

public class CreateUserHabitFromParamsCommandHandler<T> : 
    IRequestHandler<CreateUserHabitFromParamsCommand<T>, Result>
    where T : IHabitResultType
{
    private readonly IRepository<Habit<T>> _habitRepository;
    private readonly IReadRepository<HabitParams<T>> _habitParamsRepository;

    public CreateUserHabitFromParamsCommandHandler
    (
        IRepository<Habit<T>> habitRepository, 
        IReadRepository<HabitParams<T>> habitParamsRepository
    )
    {
        _habitRepository = habitRepository;
        _habitParamsRepository = habitParamsRepository;
    }

    public async Task<Result> Handle(CreateUserHabitFromParamsCommand<T> request, CancellationToken cancellationToken)
    {
        var habitParamsSpecification = new HabitParamsWithPeriodicityByIdSpecification<T>(request.HabitParamsId);
        var habitParams = await _habitParamsRepository.FirstOrDefaultAsync(habitParamsSpecification, cancellationToken);

        if (habitParams is null)
        {
            return Result.NotFound($"Habit params with id {request.HabitParamsId} was not found");
        }

        var userHabitOwner = new UserHabitOwner(request.UserId);
        var habit = new Habit<T>(Guid.NewGuid(), request.HabitParamsId, userHabitOwner);

        HabitService.AddEmptyMarksToNewlyCreatedHabit(habit, habitParams);

        await _habitRepository.AddAsync(habit, cancellationToken);

        await _habitRepository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}