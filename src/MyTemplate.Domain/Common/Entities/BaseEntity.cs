using System;

namespace MyTemplate.Domain.Common.Entities;

public abstract class BaseEntity
{
  public DateTime CreatedAt { get; set; }

  public DateTime UpdatedAt { get; set; }
}

public abstract class BaseEntity<TId> : BaseEntity where TId : notnull
{
  public TId Id { get; set; }
}