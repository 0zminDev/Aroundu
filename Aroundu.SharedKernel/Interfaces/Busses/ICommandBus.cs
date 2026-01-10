using System;
using System.Collections.Generic;
using System.Text;

namespace Aroundu.SharedKernel.Interfaces.Busses
{
    public interface ICommandBus : IDependency
    {
        Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default);
    }
}
