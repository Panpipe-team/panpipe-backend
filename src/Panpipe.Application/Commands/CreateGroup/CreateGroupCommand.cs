using Ardalis.Result;
using MediatR;

namespace Panpipe.Application.Commands.CreateGroup;

public record CreateGroupCommand(Guid UserId, string Name): IRequest<Result<Guid>>;
