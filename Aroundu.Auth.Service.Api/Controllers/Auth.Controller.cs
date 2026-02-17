using Aroundu.Auth.Service.Application.Commands;
using Aroundu.Auth.Service.Application.Queries;
using Aroundu.SharedKernel.Interfaces.Busses;
using Microsoft.AspNetCore.Mvc;

namespace Aroundu.Auth.Service.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Auth : ControllerBase
    {
        private readonly IUserQuery userQuery;
        private readonly ICommandBus commandBus;

        public Auth(IUserQuery userQuery, ICommandBus commandBus)
        {
            this.userQuery = userQuery;
            this.commandBus = commandBus;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { Message = "Auth Service is running." });
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetUsersCount()
        {
            int eventCount = await userQuery.GetUsersCountAsync();
            return Ok(new { EventCount = eventCount });
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] CreateUserCommand command)
        {
            var result = await commandBus.SendAsync(command);

            return CreatedAtAction(nameof(Get), new { id = 1 }, result);
        }
    }
}
