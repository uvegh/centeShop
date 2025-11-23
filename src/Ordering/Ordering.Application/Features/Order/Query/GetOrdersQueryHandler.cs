

using Ordering.Domain.Interface;

namespace Ordering.Application.Features.Order.Query;

public class GetOrdersQueryHandler:IRequestHandler<GetOrdersQuery,List<OrderEntity>>
{
    private readonly IOrderingRepository _orderingRepo;
    private readonly ILogger _logger;
    public GetOrdersQueryHandler(IOrderingRepository orderingRepository, ILogger<GetOrdersQueryHandler> logger )
    {
        _logger = logger;
        _orderingRepo = orderingRepository;
    }


    public async Task< List<OrderEntity>> Handle(GetOrdersQuery req, CancellationToken ct)
    {
        var orders = await _orderingRepo.GetAllAsync(ct);
        _logger.LogInformation("get orders- {orders}", orders);

        return orders;
    }

   
}
