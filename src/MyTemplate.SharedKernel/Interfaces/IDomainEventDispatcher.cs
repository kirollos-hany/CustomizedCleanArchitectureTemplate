
namespace MyTemplate.SharedKernel.Interfaces;

public interface IDomainEventDispatcher
{
  Task DispatchAndClearEvents<TId>(IEnumerable<EntityBase<TId>> entitiesWithEvents);
}
