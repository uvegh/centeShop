
using AutoMapper;
using Catalog.API.DTOs;
using Catalog.Application.Features.Command;
using Catalog.Application.Features.Query.Product;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared.Library.Model;


namespace Catalog.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
   
    private readonly IMediator _mediator;

    public ProductsController( IMediator mediator)
    {
     

        _mediator = mediator;

    }

    [HttpGet]

    public async Task<ActionResult> GetAll()
    {
      var res=  await _mediator.Send(new GetProductsQuery());
        return Ok(res);

        //return Ok(new ApiResponse<List<Product>>
        //{
        //    Success = true,
        //    Data = new GetProductsQuery()
        //    Message = "successfully retrieved"

        //});

    }

    [HttpGet("{id}")]

    public async Task<ActionResult> GetProduct([FromRoute] Guid id)
    {
        var res = await _mediator.Send(new GetProductQueryById(id));




        //    return Ok(new ApiResponse<Product>
        //    {
        //        Success = true,
        //        Data = _products[1],
        //        Message = "successfully retrieved"

        //    });
        //}
        return Ok(res);
    }

        [HttpPost]
    public async Task<ActionResult<ProductDto>> AddProduct(CreateProductCommand command)
    {
        await _mediator.Send(command);
    
        return Ok(command);
    }
}
