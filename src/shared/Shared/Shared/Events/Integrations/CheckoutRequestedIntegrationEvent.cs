

namespace Shared.Library.Events.Integrations;

public  record CheckoutRequestedIntegrationEvent
(
    Guid UserId,
        List<CartItemDto> Items

);

public record CartItemDto(
    Guid ProductId,
   int Quantity

);
