using MediatR;
using Ordering.Domain.Common;
using Shared.Library.Events;

namespace Ordering.Infrastructure.Common;

public class DomainEventDispatcher
{
    private readonly IMediator _mediator;

    public DomainEventDispatcher(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task DispatchEventAsync(IEnumerable<AggregateRoot> aggregrates)
    {
        var domainEvents = aggregrates.SelectMany(x => x.DomainEvents).ToList();


        //publish each event through mediatr
        foreach( var domainEvent in domainEvents)
        {
            //•	uses Activator.CreateInstance to build a DomainEventNotification<T> wrapper around each domain event so it can be published through MediatR.
//	Reason: domain events implements  IDomainEvent interface, but MediatR //expects an INotification(or a generic DomainEventNotification<T> type) so handlers can subscribe to DomainEventNotification<TEvent>.

            var notification = Activator.CreateInstance(typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType()),domainEvent);

            await _mediator.Publish(((INotification)notification!));
        }

        foreach(var  aggregate in aggregrates)
        {
            aggregate.ClearDomainvents();
        }
    }
}
