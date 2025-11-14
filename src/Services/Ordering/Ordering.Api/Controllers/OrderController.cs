using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Order.Command;
using Ordering.Infrastructure.Data;

namespace Ordering.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController:ControllerBase
{
    private readonly IMediator _mediator;
    public OrderController(IMediator mediator )
    {
        _mediator = mediator;

    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder( CreateOrderCommand command)

    {
      var order=  await _mediator.Send(command);
        return Ok( new { order=order, message="Order created" });
    }
}
