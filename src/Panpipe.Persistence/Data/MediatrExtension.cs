using MediatR;
using Panpipe.Domain.Entities;

namespace Panpipe.Persistence.Data;

public static class MediatrExtension
{
    public static async Task DispatchDomainEventsAsync
    (
        this IMediator mediator, ApplicationDbContext context, CancellationToken cancellationToken
    )
    {
        var domainEntities = context.ChangeTracker
            .Entries<AggregateRoot>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ToList().ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent, cancellationToken);
        }
    }
}