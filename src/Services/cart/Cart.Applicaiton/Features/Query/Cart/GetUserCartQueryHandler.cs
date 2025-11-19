


using AutoMapper;
using Cart.Domain.IRepository;
using MediatR;

namespace Cart.Application.Features.Query.Cart;

public class GetUserCartQueryHandler : IRequestHandler<GetUserCartQuery, CartDto>
{
    private readonly ILogger _logger;
    private readonly ICartRespository _cartRepo;
    private readonly IMapper _mapper;

    public GetUserCartQueryHandler(ILogger<GetUserCartQueryHandler> logger, ICartRespository cartRepo, IMapper mapper)
    {
        _logger = logger;
        _cartRepo = cartRepo;
        _mapper = mapper;

    }

  
    public async Task<CartDto> Handle( GetUserCartQuery req, CancellationToken ct)
    {
        Guid userId = req.UserId == Guid.Empty
    ? Guid.Parse("00000000-0000-0000-0000-000000000001")
    : req.UserId;
        var cart = await _cartRepo.GetByUserIdAsync(req.UserId);
        if (cart != null)
        {
            var res = _mapper.Map<CartDto>(cart);
            return res;


        }
        throw new Exception("Cart not found");

    }
}
