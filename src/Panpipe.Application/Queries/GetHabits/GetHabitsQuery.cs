using Ardalis.Result;
using MediatR;
using Panpipe.Domain.Entities.HabitAggregate;
using Panpipe.Domain.Entities.HabitParamsAggregate;

namespace Panpipe.Application.Queries.GetHabits;

public record GetHabitsQuery(Guid UserId): IRequest<Result<List<Tuple<Habit, HabitParams>>>>;
