using Aroundu.SharedKernel.Interfaces;
using Aroundu.SharedKernel.Interfaces.Busses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aroundu.Events.Service.Infrastructure.Infrastructure.MediatR
{
    public class CommandBus : ICommandBus
    {
        private readonly IMediator mediator;

        public CommandBus(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
        {
            return await mediator.Send(command, cancellationToken);
        }
    }
}
