using Catalog.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Shared.Library.Model;
using System.Xml.Linq;

namespace Catalog.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    //in-memory
    private static readonly List<Product> _products = new()
    {
        new Product {Name="Lenovo Laptop", Price=999.99m, StockQuantity=10 },
     new Product   {Name="Iphone", Price=999.99m, StockQuantity=10},
    };


    [HttpGet]

    public async Task<ActionResult <List<Product>>> GetAll()
    {
        return Ok(new ApiResponse<List<Product>>
        {
            Success = true,
            Data = _products,
            Message="successfully retrieved"

        });

    }

}