using Ardalis.Result;
using MediatR;
using Panpipe.Application.Interfaces;
using Panpipe.Application.Specifications;
using Panpipe.Domain.Entities;

namespace Panpipe.Application.Queries.GetGroups;

public class GetGroupsQueryHandler : IRequestHandler<GetGroupsQuery, Result<List<Group>>>
{
    private readonly IReadRepository<Group> _groupRepository;

    public GetGroupsQueryHandler(IReadRepository<Group> groupRepository)
    {
        _groupRepository = groupRepository;
    }
    public async Task<Result<List<Group>>> Handle(GetGroupsQuery request, CancellationToken cancellationToken)
    {
        var spec = new GroupsByParticipantUserIdSpecification(request.UserId);

        var groups = await _groupRepository.ListAsync(spec, cancellationToken);

        return Result.Success(groups);
    }
}