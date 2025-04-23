using BaMan.Application.Features.RPC.Command;
using Castle.DynamicProxy;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace BaMan.Controllers
{
    public class RPCController : Controller
    {
        private readonly IMediator _mediator;

        public RPCController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("rpc")]
        public async Task<IActionResult> Index([FromBody] RPCRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

    }
}
