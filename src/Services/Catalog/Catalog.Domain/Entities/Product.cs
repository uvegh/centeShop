

using Shared.Library.Model;

namespace Catalog.Domain.Entities
{
    public  class Product:BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
