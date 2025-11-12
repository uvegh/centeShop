

using MediatR;
using Shared.Library.Events;

namespace Ordering.Infrastructure.Common;

// This wraps any domain event into a MediatR-compatible type
public record DomainEventNotification<TDomainEvent>(TDomainEvent DomainEvent):INotification
    where TDomainEvent: BaseDomainEvent;


