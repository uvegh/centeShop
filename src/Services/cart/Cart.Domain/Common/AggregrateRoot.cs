using Shared.Library.Events;
using Shared.Library.Model;

namespace Ordering.Domain.Common;

public abstract class AggregateRoot : BaseEntity
{
    private readonly List<IDomainEvent> _domainEvents = new()
;
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);

    }
    public void ClearDomainvents()
    {
        _domainEvents.Clear();
    }
}
