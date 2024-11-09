using Ardalis.Result;
using MediatR;
using Panpipe.Domain.Entities;

namespace Panpipe.Application.Queries.GetGroups;

public record GetGroupsQuery(Guid UserId): IRequest<Result<List<Group>>>;
