using Ardalis.Specification;
using MyTemplate.Domain.Common.Interfaces;

namespace MyTemplate.Application.Interfaces.Persistence;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
{
}