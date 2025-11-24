using Cart.Application.DTOs;
using Cart.Application.Features.Command.Cart.AddItem;
using Cart.Application.Features.Command.Cart.Checkout;
using Cart.Application.Features.Command.Cart.RemoveItem;
using Cart.Application.Features.Command.Cart.UpdateItem;
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

    [HttpDelete("{userId}/item/{productId}")]

    public async Task<IActionResult> DeleteItem([FromRoute] Guid userId, [FromRoute] Guid productId)
    {

        var res = await _mediator.Send(new RemoveItemCommand(userId,productId));

        return Ok(res);
    }

    [HttpPatch("{userId}/item/{productId}")]
    public async Task<IActionResult> UpdateItem([FromRoute] Guid userId, [FromRoute] Guid productId, [FromBody] UpdateQuantityDto dto)
    {
        await _mediator.Send(new UpdateCartItemQuantityCommand(userId, productId, dto.Quantity));
        return NoContent();
    }

    [HttpPost("checkout/{userId}")]

    public async Task <IActionResult> Checkout([FromRoute] Guid userId, [FromBody] CheckOutCartCommand req)
    {
        Console.WriteLine("checkout request sent");
        var res = await _mediator.Send( new CheckOutCartCommand(userId,req.Address));
        if (res != null)
        {
            return Ok(res);
        }
        return NotFound();

    }
}
