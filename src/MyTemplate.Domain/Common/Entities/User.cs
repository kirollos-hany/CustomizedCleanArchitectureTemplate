using Microsoft.AspNetCore.Identity;
using MyTemplate.Domain.Common.Interfaces;

namespace MyTemplate.Domain.Common.Entities;

public class User : IdentityUser<Guid>, IAggregateRoot
{
  public DateTime CreatedAt { get; set; }

  public DateTime UpdatedAt { get; set; }
}

