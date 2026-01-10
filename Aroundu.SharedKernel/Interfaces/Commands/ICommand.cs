using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aroundu.SharedKernel.Interfaces
{
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }

    public interface ICommand : IRequest
    {
    }
}
