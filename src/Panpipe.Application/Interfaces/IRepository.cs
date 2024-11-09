using Ardalis.Specification;
using Panpipe.Domain.Entities;

namespace Panpipe.Application.Interfaces;

public interface IRepository<T>: IRepositoryBase<T> where T: AggregateRoot {}
