

using Ordering.Infrastructure.Data;
using Ordering.Domain.Interface;



namespace Ordering.Application.Features.Order.Command;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly OrderingDbContext _context;
    private readonly ILogger<CreateOrderCommandHandler> _logger;
    private readonly IOrderingRepository _orderingRepo;
    public CreateOrderCommandHandler(OrderingDbContext context, ILogger<CreateOrderCommandHandler> logger, IOrderingRepository orderingRepo)
    {
        _context = context;
        _logger = logger;
        _orderingRepo = orderingRepo;
    }

    public async Task<Guid> Handle( CreateOrderCommand req ,CancellationToken ct)
    {
        // call the create function  added in the order entity
       
        var order = OrderEntity.Create(
     req.UserId, req.TotalAmount,req.Items,req.Status);
       await  _orderingRepo.AddAsync(order,ct);
        //_context.Add(order);
        //await _context.SaveChangesAsync(ct);// calls the overriden method created to add all domain and aggregates

        _logger.LogInformation("order created, new id -{Id}", order.Id);


        return order.Id;
    }

  
}
