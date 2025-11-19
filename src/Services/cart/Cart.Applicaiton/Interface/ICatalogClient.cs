

using Catalog.API.DTOs;

namespace Cart.Application.Interface;

public  interface ICatalogClient
{
    Task<ProductDto?> GetProduct(Guid id,CancellationToken ct);
}
