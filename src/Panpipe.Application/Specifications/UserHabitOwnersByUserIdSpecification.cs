using Ardalis.Specification;
using Panpipe.Domain.Entities.HabitOwnerAggregate;

namespace Panpipe.Application.Specifications;

public class UserHabitOwnersByUserIdSpecification: Specification<UserHabitOwner>
{
    public UserHabitOwnersByUserIdSpecification(Guid userId)
    {
        Query
            .Where(userHabitOwner => userHabitOwner.UserId == userId);
    }
}
