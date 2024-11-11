using Ardalis.Result;
using MediatR;

namespace Panpipe.Application.Commands.CreateUserHabitFromParams;

public record CreateUserHabitFromParamsCommand(Guid UserId, Guid HabitParamsId): IRequest<Result>;
