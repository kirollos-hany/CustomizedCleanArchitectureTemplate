using MyTemplate.Application.Dtos.Entity;

namespace MyTemplate.Application.Mappings.Entity;

public static class EntityMapping
{
  public static EntityDto ToDto(this Domain.Entities.Entity entity) => new EntityDto
  {
    CreatedAt = entity.CreatedAt,
    Id = entity.Id,
    UpdatedAt = entity.UpdatedAt
  };
}