using Ardalis.Specification.EntityFrameworkCore;
using MyTemplate.Application.Interfaces.Persistence;
using MyTemplate.Domain.Common.Interfaces;

namespace MyTemplate.Infrastructure.Data;

// inherit from Ardalis.Specification type
public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
{
  public EfRepository(AppDbContext dbContext) : base(dbContext)
  {
  }
}
