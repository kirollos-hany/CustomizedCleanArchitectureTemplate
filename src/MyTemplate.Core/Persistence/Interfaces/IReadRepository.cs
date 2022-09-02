using Ardalis.Specification;

namespace MyTemplate.Core.Persistence.Interfaces;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
{
}
