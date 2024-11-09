using Ardalis.Specification;
using Panpipe.Domain.Entities;

namespace Panpipe.Application.Specifications;

public class GroupsByParticipantUserIdSpecification: Specification<Group>
{
    public GroupsByParticipantUserIdSpecification(Guid userId)
    {
        Query
            .Where(group => group.UserIds.Contains(userId));
    }
}