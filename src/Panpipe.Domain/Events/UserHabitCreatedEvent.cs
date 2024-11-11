using Panpipe.Domain.Interfaces;

namespace Panpipe.Domain.Events;

public record UserHabitCreatedEvent(Guid HabitId, Guid UserId): IDomainEvent;
