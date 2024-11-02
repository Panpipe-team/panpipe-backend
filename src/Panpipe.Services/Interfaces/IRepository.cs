using Ardalis.Specification;

using Panpipe.Domain.Interfaces;

namespace Panpipe.Services.Interfaces;

public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot { }
