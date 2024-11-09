using MediatR;
using Ardalis.Result;
using Panpipe.Domain.Interfaces;

namespace Panpipe.Application.Commands.ChangeHabitResult;

public record ChangeHabitResultCommand<T>(Guid HabitId, Guid MarkId, T Value): IRequest<Result> 
    where T: IHabitResultType;
