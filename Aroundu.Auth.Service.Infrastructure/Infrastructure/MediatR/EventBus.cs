using Aroundu.SharedKernel.Interfaces;
using Aroundu.SharedKernel.Interfaces.Busses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aroundu.Auth.Service.Infrastructure.Infrastructure.MediatR
{
    public class EventBus : IEventBus
    {
        private readonly IMediator mediator;

        public EventBus(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task PublishAsync<T>(T domainEvent, CancellationToken cancellationToken = default) where T : IEvent
        {
            await mediator.Publish(domainEvent, cancellationToken);
        }
    }
}
