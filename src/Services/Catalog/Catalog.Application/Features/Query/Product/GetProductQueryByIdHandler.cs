



using Catalog.Domain.Interfaces;


namespace Catalog.Application.Features.Query.Product;

public class GetProductQueryByIdHandler :IRequestHandler<GetProductQueryById, ProductDto>
{
    private readonly IProductRepository _productRepo;
    private readonly ILogger _logger;
    private IMapper _mapper;


public GetProductQueryByIdHandler(IProductRepository productRepository, ILogger<GetProductQueryByIdHandler> logger, IMapper mapper)
{
        _logger = logger;
        _productRepo = productRepository;
        _mapper = mapper;

}




    public async Task<ProductDto> Handle( GetProductQueryById req, CancellationToken ct)
    {
        var product = await  _productRepo.GetByIdAsync(req.id);
        if (product != null)
        {
            var res = _mapper.Map<ProductDto>(product);
            return res;
        }
        throw new Exception("Product not found");
    }
}
