using System;
using System.Collections.Generic;
using System.Text;

namespace Aroundu.SharedKernel.Interfaces.Busses
{
    public interface IEventBus : IDependency
    {
        Task PublishAsync<T>(T domainEvent, CancellationToken cancellationToken = default) where T : IEvent;
    }
}
