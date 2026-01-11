using MediatR;

namespace Aroundu.SharedKernel.Interfaces
{
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }

    public interface ICommand : IRequest
    {
    }
}
