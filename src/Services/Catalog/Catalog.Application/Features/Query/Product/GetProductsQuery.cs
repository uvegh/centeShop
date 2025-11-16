



using Catalog.API.DTOs;

namespace Catalog.Application.Features.Query.Product;

public record class GetProductsQuery():IRequest<List<ProductDto>>;

