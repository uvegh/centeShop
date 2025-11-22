
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Order.Command;

namespace Ordering.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController:ControllerBase
    {
        
     
        private readonly IMediator _mediator;
        public OrderController( IMediator mediator)
        {
            _mediator = mediator; 

     
        }

        [HttpPost]

       public async Task<ActionResult<Guid>> Create(CreateOrderCommand command)
        {
        var res=    await _mediator.Send(command);
            return Ok(res);

        }

       
        

    }
}
