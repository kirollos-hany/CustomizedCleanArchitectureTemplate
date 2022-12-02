using Ardalis.Specification;
using MyTemplate.Domain.Common.Interfaces;

namespace MyTemplate.Application.Interfaces.Persistence;

public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
{
    
}