



namespace Catalog.Application.Features.Query.Product;

public record  GetProductQueryById(Guid id):IRequest<ProductDto>;

