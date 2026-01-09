using Aroundu.Events.Service.Application.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using Aroundu.Events.Service.Application.Commands;

namespace Aroundu.Events.Service.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Events : ControllerBase
    {
        private readonly IEventQuery eventsQuery;
        private readonly IMediator mediator;
        public Events(IEventQuery eventsQuery, IMediator mediator)
        {
            this.eventsQuery = eventsQuery;
            this.mediator = mediator;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { Message = "Events Service is running." });
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetEventCount()
        {
            int eventCount = await eventsQuery.GetEventCountAsync();
            return Ok(new { EventCount = eventCount });
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventCommand command)
        {
            var result = await mediator.Send(command);

            return CreatedAtAction(nameof(Get), new { id = 1 }, result);
        }
    }
}
