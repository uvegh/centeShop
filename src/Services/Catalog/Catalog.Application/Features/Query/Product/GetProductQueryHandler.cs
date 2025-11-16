



using AutoMapper;
using Catalog.API.DTOs;
using Catalog.Domain.Interfaces;
using Catalog.Infrastructure.Data;

namespace Catalog.Application.Features.Query.Product;

public class GetProductQueryHandler :IRequestHandler<GetProductsQuery,List<ProductDto>>
{
  
    private readonly ILogger _logger;
    private readonly IProductRepository _productRepo;
    private readonly IMapper _mapper;
    public GetProductQueryHandler( CatalogDbContext dbContext, ILogger<GetProductQueryHandler>logger, IProductRepository productRepo, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
        _productRepo = productRepo;
    }

    public async Task<List<ProductDto>> Handle(GetProductsQuery req, CancellationToken ct)
    {
        var products = await  _productRepo.GetAllAsync(ct);
        var res= _mapper.Map <List<ProductDto>>(products);

        _logger.LogInformation("get all products - {res}",res);
        return res;

    }

   
}