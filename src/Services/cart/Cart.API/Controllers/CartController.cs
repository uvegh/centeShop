using Cart.Application.Features.Cart.Command.AddItem;
using MassTransit.Mediator;
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

    public async Task<IActionResult> Add(AddToCartCommand command)
    {
        await _mediator.Publish(command);

        return Ok("Item added");
    }

}
