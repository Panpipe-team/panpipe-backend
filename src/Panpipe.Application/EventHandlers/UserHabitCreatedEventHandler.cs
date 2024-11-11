using MediatR;
using Panpipe.Application.Interfaces;
using Panpipe.Domain.Entities.HabitOwnerAggregate;
using Panpipe.Domain.Events;

namespace Panpipe.Application.EventHandlers;

public class UserHabitCreatedEventHandler : INotificationHandler<UserHabitCreatedEvent>
{
    private readonly IRepository<UserHabitOwner> _userHabitOwnerRepository;

    public UserHabitCreatedEventHandler(IRepository<UserHabitOwner> repository)
    {
        _userHabitOwnerRepository = repository;
    }

    public async Task Handle(UserHabitCreatedEvent notification, CancellationToken cancellationToken)
    {
        var userHabitOwner = new UserHabitOwner(Guid.NewGuid(), notification.UserId, notification.HabitId);

        await _userHabitOwnerRepository.AddAsync(userHabitOwner, cancellationToken);
    }
}
