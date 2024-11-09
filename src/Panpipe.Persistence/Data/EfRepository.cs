using Ardalis.Specification.EntityFrameworkCore;
using Panpipe.Application.Interfaces;
using Panpipe.Domain.Entities;

namespace Panpipe.Persistence.Data;

public class EfRepository<T>: RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T: AggregateRoot
{
    public EfRepository(ApplicationDbContext dbContext): base(dbContext) {}
}
