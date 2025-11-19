using Cart.Application.Features.Command.Cart.AddItem;
using Cart.Application.Features.Query.Cart;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cart.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController:ControllerBase
{
    private readonly IMediator _mediator;
    public CartController(IMediator mediator)
    {
        _mediator = mediator;
        
    }

    [HttpPost("add")]

    public async Task<IActionResult> Add(AddToCartCommand req)
    {
      var res=    await _mediator.Send(new AddToCartCommand(req.UserId, req.ProductId, req.Quantity));

        return Ok(res);
    }

    [HttpGet("{id}")]

    public async Task<IActionResult> GetCart(  [FromRoute] Guid id)
    {
        var res = await _mediator.Send(new  GetUserCartQuery(id));

        return Ok(res);
    }

}
