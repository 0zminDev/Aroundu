using Aroundu.SharedKernel.IntegrationEvents;
using Aroundu.SharedKernel.Interfaces.Events;
using Microsoft.Extensions.Logging;

namespace Aroundu.Events.Service.Application.Consumers
{
    public class UserCreatedIntegrationEventHandler : IIntegrationEventHandler<UserCreatedEvent>
    {
        private readonly ILogger<UserCreatedIntegrationEventHandler> logger;

        public UserCreatedIntegrationEventHandler(ILogger<UserCreatedIntegrationEventHandler> logger)
        {
            this.logger = logger;
        }

        public async Task Handle(UserCreatedEvent @event, CancellationToken ct)
        {
            logger.LogInformation("Handling UserCreatedEvent for UserId: {UserId}, Username: {Username}, Email: {Email}",
                @event.UserId, @event.Username, @event.Email);
            await Task.CompletedTask;
        }
    }
}
