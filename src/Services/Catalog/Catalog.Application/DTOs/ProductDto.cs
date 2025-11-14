namespace Catalog.API.DTOs
{
    public record ProductDto(Guid Id
        , string Name,
        decimal Price, 
        int StockQuantity);

    public record CreateProductDto(string Name,
        decimal Price, 
        int StockQuantity);
}
