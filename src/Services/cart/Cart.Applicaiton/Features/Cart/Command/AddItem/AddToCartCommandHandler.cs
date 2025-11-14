
using CartEntity= Cart.Domain.Entities.Cart;
using Cart.Domain.IRepository;

namespace Cart.Application.Features.Cart.Command.AddItem;

public class AddToCartCommandHandler : IRequestHandler< AddToCartCommand>
{

    private readonly ICartRespository  _cartRespository;

    public AddToCartCommandHandler(ICartRespository cartRespository)
    {
        _cartRespository = cartRespository;

    }

    public async Task Handle(AddToCartCommand request, CancellationToken ct)
    {
        //get user cart, if doesnot exist   create one else add items,

        var cartExist = await _cartRespository.GetByUserIdAsync(request.UserId);
        if (cartExist != null)
        {
            cartExist.AddItem(request.ProductId, request.Quantity);
        }
        
        
            //create
            CartEntity.Create(request.UserId);

        
    }


}
