using MediatR;
using Panpipe.Application.Interfaces;
using Panpipe.Domain.Entities.HabitAggregate;
using Panpipe.Domain.Interfaces;
using Panpipe.Application.Specifications;
using Ardalis.Result;

namespace Panpipe.Application.Commands.ChangeHabitResult;

public class ChangeHabitResultCommandHandler<T> : IRequestHandler<ChangeHabitResultCommand<T>, Result> 
    where T : IHabitResultType
{
    private readonly IRepository<AbstractHabit<T>> _habitRepository;

    public ChangeHabitResultCommandHandler(IRepository<AbstractHabit<T>> habitRepository)
    {
        _habitRepository = habitRepository;
    }

    public async Task<Result> Handle(ChangeHabitResultCommand<T> request, CancellationToken cancellationToken)
    {
        var specification = new HabitWithMarksSpecification<T>(request.HabitId);

        var habit = await _habitRepository.FirstOrDefaultAsync(specification, cancellationToken);
        
        if (habit is null)
        {
            return Result.NotFound($"Habit with id {request.HabitId} was not found");
        }
        
        try {
            habit.ChangeResult(request.MarkId, request.Value);
        }
        catch (InvalidOperationException) {
            return Result.NotFound($"Mark with id {request.MarkId} was not found");
        }

        await _habitRepository.UpdateAsync(habit, cancellationToken);

        await _habitRepository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}