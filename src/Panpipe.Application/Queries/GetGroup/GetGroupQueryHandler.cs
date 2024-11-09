using Ardalis.Result;
using MediatR;
using Panpipe.Application.Interfaces;
using Panpipe.Domain.Entities;

namespace Panpipe.Application.Queries.GetGroup;

public class GetGroupQueryHandler : IRequestHandler<GetGroupQuery, Result<Group>>
{
    private readonly IReadRepository<Group> _groupRepository;

    public GetGroupQueryHandler(IReadRepository<Group> groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task<Result<Group>> Handle(GetGroupQuery request, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetByIdAsync(request.Id);

        if (group == null)
        {
            return Result.NotFound();
        }

        return Result.Success(group);
    }
}
