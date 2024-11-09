using Ardalis.Specification;
using Panpipe.Domain.Entities;

namespace Panpipe.Application.Interfaces;

public interface IReadRepository<T>: IReadRepositoryBase<T> where T: AggregateRoot {}
