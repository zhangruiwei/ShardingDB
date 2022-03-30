using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Warren.OrderService.Application.Models;

namespace Warren.OrderService.API.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("order/create")]
        public async Task<IApiResult> QueryOrderInfo([FromBody] Application.Commands.CreateOrderCommand.CreateOrderCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPost("order/page")]
        public async Task<OkApiResult<List<Application.Commands.QueryOrderByPagingCommand.QueryOrderByPagingCommandResponse>>> QueryOrderByPaging([FromBody] Application.Commands.QueryOrderByPagingCommand.QueryOrderByPagingCommand command)
        {
            return await _mediator.Send(command);
        }

    }
}
