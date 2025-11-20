

using Cart.Application.Features.Command.Cart.RemoveItem;
using Cart.Domain.IRepository;

namespace Cart.Application.Features.Command.Cart.RemoveItem;

public class RemoveItemCommandHandler:IRequestHandler<RemoveItemCommand,bool>
{
    private readonly ICartRespository _cartRepo;
    private readonly ILogger _logger;

    public RemoveItemCommandHandler(ILogger<RemoveItemCommandHandler> logger, ICartRespository cartRespository)
    {
        _cartRepo = cartRespository;
        _logger = logger;
    }
    public async Task<bool> Handle(RemoveItemCommand req, CancellationToken ct)
    {
        var cart = await _cartRepo.GetByUserIdAsync(req.UserId);
        if (cart != null)
        {
            var item = cart.Items.FirstOrDefault(x => x.ProductId == req.ProductId);
            if (item != null)
            {


                await _cartRepo.RemoveCartItemAsync(req.UserId, req.ProductId);
                return true;
            }
            return false;
        }
        
        return false;


        
    }
}
