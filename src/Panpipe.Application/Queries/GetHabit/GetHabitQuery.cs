using Ardalis.Result;
using MediatR;
using Panpipe.Domain.Entities.HabitAggregate;
using Panpipe.Domain.Entities.HabitParamsAggregate;

namespace Panpipe.Application.Queries.GetHabit;

public record GetHabitQuery(Guid HabitId): IRequest<Result<Tuple<Habit, HabitParams>>>;
