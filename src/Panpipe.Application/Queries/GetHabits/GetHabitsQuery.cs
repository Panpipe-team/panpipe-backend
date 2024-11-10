using Ardalis.Result;
using MediatR;
using Panpipe.Domain.Entities.HabitAggregate;
using Panpipe.Domain.Interfaces;

namespace Panpipe.Application.Queries.GetHabits;

public record GetHabitsQuery<T>(Guid UserId): IRequest<Result<List<AbstractHabit<T>>>> where T: IHabitResultType;
