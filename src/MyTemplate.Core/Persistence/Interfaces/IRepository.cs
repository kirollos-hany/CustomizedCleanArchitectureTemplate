using Ardalis.Specification;

namespace MyTemplate.Core.Persistence.Interfaces;

// from Ardalis.Specification
public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
{
}
