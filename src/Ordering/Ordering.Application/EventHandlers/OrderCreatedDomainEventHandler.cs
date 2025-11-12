using Microsoft.Extensions.Logging;
using MassTransit;

using MediatR;
using Ordering.Infrastructure.Common;
using Ordering.Domain.Events;
using Shared.Library.Events;

namespace Ordering.Application.EventHandlers;
//mediatr automatically detects this after attaching the notificationhandler and with reference to the domaainevenr
public class OrderCreatedDomainEventHandler:INotificationHandler<DomainEventNotification<OrderCreatedDomainEvent>>
{
    private readonly ILogger<OrderCreatedDomainEventHandler> _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    public OrderCreatedDomainEventHandler(ILogger<OrderCreatedDomainEventHandler> logger, IPublishEndpoint publishEndpoint )
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;



    }

    //event handler
    public async Task Handle(DomainEventNotification<OrderCreatedDomainEvent> notification, CancellationToken ct)
    {
        var @event = notification.DomainEvent;
        _logger.LogInformation("order created domain event notification, orderId={OrderId} and occured at {OccuredAt}", @event.OrderId, @event.OccuredAt);

        var integrationEvent = new PaymentCreatedIntegrationEvent(@event.OrderId, @event.UserId, @event.TotalAmount);
        _logger.LogInformation("Publish payment creation event order {OrderId} at {OccurredAt}", @event.OrderId, @event.OccuredAt);
        await _publishEndpoint.Publish(integrationEvent, ct);
        //set task to completed after logging
        await Task.CompletedTask;
    }
 
    
}
