using Aroundu.Events.Service.Application.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Aroundu.Events.Service.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Events : ControllerBase
    {
        private readonly IEventQuery eventsQuery;
        public Events(IEventQuery eventsQuery)
        {
            this.eventsQuery = eventsQuery;
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
    }
}
