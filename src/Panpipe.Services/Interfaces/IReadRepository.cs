using Ardalis.Specification;

using Panpipe.Domain.Interfaces;

namespace Panpipe.Services.Interfaces;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot { }
