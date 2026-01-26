using Aroundu.Events.Service.Application.Queries;
using Microsoft.AspNetCore.Mvc;
using Aroundu.Events.Service.Application.Commands;
using Aroundu.SharedKernel.Interfaces.Busses;

namespace Aroundu.Events.Service.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Events : ControllerBase
    {
        private readonly IEventQuery eventsQuery;
        private readonly ICommandBus commandBus;

        public Events(IEventQuery eventsQuery, ICommandBus commandBus)
        {
            this.eventsQuery = eventsQuery;
            this.commandBus = commandBus;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { Message = "Events Service is running." });
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetEventCount()
        {
            int eventCount = await eventsQuery.GetEventsCountAsync();
            return Ok(new { EventCount = eventCount });
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventCommand command)
        {
            var result = await commandBus.SendAsync(command);

            return CreatedAtAction(nameof(Get), new { publicKey = new Guid() }, result);
        }
    }
}
