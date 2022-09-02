using Ardalis.Specification.EntityFrameworkCore;
using MyTemplate.Core.Persistence.Interfaces;

namespace MyTemplate.Infrastructure.Data;

// inherit from Ardalis.Specification type
public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
{
  public EfRepository(ApplicationDbContext dbContext) : base(dbContext)
  {
  }
}
