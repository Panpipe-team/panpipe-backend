using Ardalis.Result;
using MediatR;
using Panpipe.Domain.Entities.HabitAggregate;
using Panpipe.Domain.Interfaces;

namespace Panpipe.Application.Queries.GetHabit;

public record GetHabitQuery<T>(Guid HabitId): IRequest<Result<Habit<T>>> where T: IHabitResultType;
