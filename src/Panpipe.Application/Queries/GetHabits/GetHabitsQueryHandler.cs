using Ardalis.Result;
using MediatR;
using Panpipe.Application.Interfaces;
using Panpipe.Application.Specifications;
using Panpipe.Domain.Entities.HabitAggregate;
using Panpipe.Domain.Entities.HabitOwnerAggregate;
using Panpipe.Domain.Entities.HabitParamsAggregate;

namespace Panpipe.Application.Queries.GetHabits;

public class GetHabitsQueryHandler : IRequestHandler<GetHabitsQuery, Result<List<Tuple<Habit, HabitParams>>>>
{
    private readonly IReadRepository<Habit> _habitRepository;
    private readonly IReadRepository<HabitParams> _habitParamsRepository;
    private readonly IReadRepository<UserHabitOwner> _userHabitOwnerRepository;

    public GetHabitsQueryHandler(
        IReadRepository<Habit> habitRepository, 
        IReadRepository<HabitParams> habitParamsRepository,
        IReadRepository<UserHabitOwner> userHabitOwnerRepository
    )
    {
        _habitRepository = habitRepository;
        _habitParamsRepository = habitParamsRepository;
        _userHabitOwnerRepository = userHabitOwnerRepository;
    }
    
    public async Task<Result<List<Tuple<Habit, HabitParams>>>> Handle(
        GetHabitsQuery request, CancellationToken cancellationToken
    )
    {
        var userHabitOwnersSpec = new UserHabitOwnersByUserIdSpecification(request.UserId);
        var userHabitOwners = await _userHabitOwnerRepository.ListAsync(userHabitOwnersSpec, cancellationToken);

        var habitIds = userHabitOwners.Select(x => x.HabitId).ToList();
        var habitsSpec = new HabitByIdsSpecification(habitIds);
        var habits = await _habitRepository.ListAsync(habitsSpec, cancellationToken);

        var habitParamsIds = habits.Select(x => x.ParamsId).ToList();
        var habitParamsSpec = new HabitParamsByIdsSpecification(habitParamsIds);
        var habitParamsList = await _habitParamsRepository.ListAsync(habitParamsSpec, cancellationToken);

        var result = new List<Tuple<Habit, HabitParams>>();
        foreach (var habit in habits)
        {
            var habitParams = habitParamsList.FirstOrDefault(x => x.Id == habit.ParamsId);

            if (habitParams is null)
            {
                return Result.CriticalError(
                    $"Habit params with id {habit.ParamsId} for found habit with id {habit.Id} was not found"
                );
            }

            result.Add(new Tuple<Habit, HabitParams>(habit, habitParams));
        }

        return Result.Success(result);
    }
}
