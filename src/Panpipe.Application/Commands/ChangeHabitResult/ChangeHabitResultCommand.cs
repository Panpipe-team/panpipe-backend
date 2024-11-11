using MediatR;
using Ardalis.Result;

namespace Panpipe.Application.Commands.ChangeHabitResult;

public record ChangeHabitResultCommand(Guid HabitId, Guid MarkId, string Value): IRequest<Result>;
