using Aroundu.SharedKernel.Interfaces;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aroundu.Events.Service.Infrastructure.Infrastructure.MassTransit
{
    public class ServiceEventBus : IServiceEventBus
    {
        private readonly IPublishEndpoint publishEndpoint;

        public ServiceEventBus(IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint;
        }

        public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
        {
            await publishEndpoint.Publish(message, cancellationToken);
        }
    }
}
