using Ardalis.Result;
using MediatR;
using Panpipe.Domain.Interfaces;

namespace Panpipe.Application.Commands.CreateUserHabitFromParams;

public record CreateUserHabitFromParamsCommand<T>(Guid UserId, Guid HabitParamsId): IRequest<Result> 
    where T: IHabitResultType;
