using Ardalis.Specification.EntityFrameworkCore;
using MediatR;
using Panpipe.Application.Interfaces;
using Panpipe.Domain.Entities;

namespace Panpipe.Persistence.Data;

public class EfRepository<T>: RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T: AggregateRoot
{
    private readonly IMediator _mediator;
    private readonly ApplicationDbContext _dbContext;

    public EfRepository(ApplicationDbContext dbContext, IMediator mediator): base(dbContext)
    {
        _mediator = mediator;
        _dbContext = dbContext;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEventsAsync(_dbContext, cancellationToken);

        return await base.SaveChangesAsync(cancellationToken);
    }
}
