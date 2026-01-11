using Aroundu.SharedKernel.Interfaces;
using Aroundu.SharedKernel.Interfaces.Events;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aroundu.Events.Service.Infrastructure.Infrastructure.MassTransit
{
    public class MassTransitIntegrationEventHandlerAdapter<TEvent> : IConsumer<TEvent>
        where TEvent : class, IIntegrationEvent
    {
        private readonly IServiceProvider serviceProvider;

        public MassTransitIntegrationEventHandlerAdapter(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task Consume(ConsumeContext<TEvent> context)
        {
            var handler = serviceProvider.GetService<IIntegrationEventHandler<TEvent>>();

            if (handler != null)
            {
                await handler.Handle(context.Message, context.CancellationToken);
            }
        }
    }
}
