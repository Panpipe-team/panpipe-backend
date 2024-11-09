using Ardalis.Result;
using MediatR;
using Panpipe.Domain.Entities;

namespace Panpipe.Application.Queries.GetGroup;

public record GetGroupQuery(Guid Id): IRequest<Result<Group>>;
